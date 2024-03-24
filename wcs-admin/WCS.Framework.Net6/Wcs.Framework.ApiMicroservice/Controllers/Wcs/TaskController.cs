using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Const;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Core;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.Service;
using Wcs.Framework.WebCore;
using Wcs.Framework.WebCore.AttributeExtend;
using Wcs.Framework.WebCore.AuthorizationPolicy;

namespace Wcs.Framework.ApiMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : BaseSimpleCrudController<TaskEntity>
    {
        /// <summary>
        /// 入库模式标识
        /// </summary>
        const string rkType = "002cfc70-89a2-11ed-a5d9-00163e10b48b";
        /// <summary>
        /// 出库模式标识
        /// </summary>
        const string ckType = "ba45546e-915b-11ed-a5d9-00163e10b48b";

        /// <summary>
        /// 执行机IP
        /// </summary>
        //const string wcsConsoleServerHost = "http://172.19.48.155:5005";
        const string wcsConsoleServerHost = "http://127.0.0.1:5005";

        private readonly ITaskService _iTaskService;
        private readonly IShelvesPositionService _iShelvesPositionService;
        private readonly IPointMapService _iPointMapService;
        private readonly IEquipmentService _iEquipmentService;
        private QuartzInvoker _quartzInvoker;
        private CacheInvoker _cacheDb;
        private readonly IMapper _mapper;

        public TaskController(
            ILogger<TaskEntity> logger,
            ITaskService iTaskService,
            IShelvesPositionService iShelvesPositionService,
            IPointMapService iPointMapService,
            IEquipmentService iEquipmentService,
            QuartzInvoker quartzInvoker,
            CacheInvoker cacheInvoker,
            IMapper mapper
        ) : base(logger, iTaskService)
        {
            this._mapper = mapper;
            this._quartzInvoker = quartzInvoker;
            this._iTaskService = iTaskService;
            this._iPointMapService = iPointMapService;
            this._iEquipmentService = iEquipmentService;
            this._iShelvesPositionService = iShelvesPositionService;
            this._cacheDb = cacheInvoker;
        }

        /// <summary>
        /// 获取设备任务列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] TaskEntity entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iTaskService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> Delete([FromQuery] string id)
        {
            TaskEntity task = await _iTaskService.GetDetailById(id);
            if (task.task_type == rkType)
            {
                // 先从执行机上解除相关货位状态
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/CancelTask?hwId={task.end_object}");

                ConsoleHttpResult consoleHttpResult = JsonHelper.ParseFormByJson<ConsoleHttpResult>(result);

                if (consoleHttpResult.status)
                {
                    if (task.end_object != null)
                    {
                        // 更新货位信息
                        await _iShelvesPositionService.UpdateEntity(new ShelvesPositionEntity()
                        {
                            Id = task.end_object,
                            is_loaded = false,
                            is_locked = false
                        });
                    }
                    // 删除任务信息
                    await _iTaskService.DeleteById(id);
                }
            }
            //string result = await HttpHelper.Get($"http://localhost:5005/api/console/CancelTask?hwId={task.end_object}");
            //CancelTask
            return Result.Success();
        }

        /// <summary>
        /// 启动一个定时任务,自动入库
        /// 10秒调用一次
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> AutoRkModeStart()
        {
            string taskName = "auto-rk-mode";
            string taskGroup = "wcs";
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {JobConst.method,"get" },
                {JobConst.url,$"{wcsConsoleServerHost}/api/console/AutoMode" }
            };

            await _quartzInvoker.StartAsync("*/10 * * * * ?", "HttpJob", jobName: taskName, jobGroup: taskGroup, data: data);

            return Result.Success();
        }

        /// <summary>
        /// 停止任务,停止自动入库
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> AutoRkModeStop()
        {
            await _quartzInvoker.StopAsync(new Quartz.JobKey("auto-rk-mode", "wcs"));
            await _quartzInvoker.DeleteAsync(new Quartz.JobKey("auto-rk-mode", "wcs"));

            return Result.Success();
        }

        /// <summary>
        /// 启动一个定时任务,启动任务处理
        /// 10秒调用一次
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> DealTaskStart()
        {
            string taskName = "auto-deal-task";
            string taskGroup = "wcs";
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {JobConst.method,"get" },
                {JobConst.url,$"{wcsConsoleServerHost}/api/console/DealTask" }
            };

            await _quartzInvoker.StartAsync("*/10 * * * * ?", "HttpJob", jobName: taskName, jobGroup: taskGroup, data: data);

            return Result.Success();
        }

        /// <summary>
        /// 停止任务,停止执行任务
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> DealTaskStop()
        {
            await _quartzInvoker.StopAsync(new Quartz.JobKey("auto-deal-task", "wcs"));
            await _quartzInvoker.DeleteAsync(new Quartz.JobKey("auto-deal-task", "wcs"));

            return Result.Success();
        }

        /// <summary>
        /// 获取所有设备对象
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> AllObjects()
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/AllObjects");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }

        /// <summary>
        /// 手工创建入库任务-依赖执行机
        /// </summary>
        /// <param name="startObject"></param>
        /// <param name="endObject"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> ManualRkMode([FromQuery] string startObject, [FromQuery] string endObject)
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/ManualRkMode?startObject={startObject}&endObject={endObject}");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }

        /// <summary>
        /// 手工创建出库任务-依赖执行机
        /// </summary>
        /// <param name="startObject"></param>
        /// <param name="endObject"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> ManualCkMode([FromQuery] string startObject, [FromQuery] string endObject)
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/ManualCkMode?startObject={startObject}&endObject={endObject}");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }

        /// <summary>
        /// 获取执行机上信息
        /// </summary>
        /// <param name="type">INFO/ERROR</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetServerMessages([FromQuery] string type)
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/GetServerMessages?type={type}");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }

        /// <summary>
        /// 任务统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> TaskStatistics()
        {
            TaskStatisticsDto result = _cacheDb.Get<TaskStatisticsDto>($"Yi:TaskStatistics");

            if (result == null)
            {
                result = await _iTaskService.TaskStatistics();

                _cacheDb.Set($"Yi:TaskStatistics", result, new TimeSpan(0, 0, 10));
            }

            return Result.Success().SetData(await _iTaskService.TaskStatistics());
        }

        /// <summary>
        /// 仓库数据初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> InitAllData()
        {
            // 1、初始化映射点位
            // 获取所有x真实坐标值
            // 获取所有y真实坐标值
            List<int?> pointX = await _iEquipmentService.GetAllPointXReal();
            List<int?> pointY = await _iEquipmentService.GetAllPointYReal();

            pointX.AddRange(await _iShelvesPositionService.GetAllPointXReal());
            pointY.AddRange(await _iShelvesPositionService.GetAllPointYReal());

            pointX = pointX.Distinct().ToList();
            pointY = pointY.Distinct().ToList();

            pointX.Sort();
            pointY.Sort();

            await _iPointMapService.ClearTable();

            for (int index = 1; index < pointX.Count; index++)
            {
                PointMapEntity point = new PointMapEntity()
                {
                    point_type = "x",
                    value = index - 0,
                    min_realy = pointX[index - 1],
                    max_realy = pointX[index]
                };
                await _iPointMapService.AddEntity(point);
            }

            for (int index = 1; index < pointY.Count; index++)
            {
                PointMapEntity point = new PointMapEntity()
                {
                    point_type = "y",
                    value = index - 0,
                    min_realy = pointY[index - 1],
                    max_realy = pointY[index]
                };
                await _iPointMapService.AddEntity(point);
            }

            // 2、清空仓库任务
            await _iTaskService.InitTask();
            // 3、货位信息初始化
            await _iShelvesPositionService.InitLockedLoaded();

            string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/InitAllData");

            return Result.Success();
        }

        /// <summary>
        /// 获取最新的任务执行信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetTop3Task()
        {
            List<TaskDto> tasks = await _iTaskService.GetTop3Task();

            List<string> messages = new List<string>();

            tasks.ForEach(item =>
            {
                DateTime dateTime = (DateTime)item.CreateTime;

                messages.Insert(0, $"[{dateTime.ToString("yyyyMMddmmss")}]{item.task_type}-{item.task_status}");
            });

            return Result.Success().SetData(new { messages });
        }

        /// <summary>
        /// 查询可运行设备状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetEquipmentStatus()
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/GetEquipmentStatus");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }

        /// <summary>
        /// 通知执行机更新货位信息
        /// </summary>
        /// <param name="hwId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> UpdateShelvesPosition([FromQuery] string hwId, [FromQuery] int status)
        {
            try
            {
                string result = await HttpHelper.Get($"{wcsConsoleServerHost}/api/console/UpdateShelvesPosition?hwId={hwId}&status={status}");

                return Result.Success().SetData(Newtonsoft.Json.JsonConvert.DeserializeObject(result));
            }
            catch (Exception ex)
            {
                return Result.SuccessError("执行机未开启");
            }
        }
    }
}
