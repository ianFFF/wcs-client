using System;
using Microsoft.AspNetCore.Mvc;
using wcs.core;
using wcs.core.@interface;
using wcs.core.model;
using wcs.core.common;
using wcs.core.service;
using Microsoft.VisualBasic;
using System.Runtime.Intrinsics.X86;

namespace wcs.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConsoleController : ControllerBase
    {
        // 入库模式标识
        const string rkType = "002cfc70-89a2-11ed-a5d9-00163e10b48b";
        // 出库模式标识
        const string ckType = "ba45546e-915b-11ed-a5d9-00163e10b48b";

        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AllObjects()
        {
            List<IEquipmentObject> result = new List<IEquipmentObject>();

            List<IEquipmentObject> allObjetcs = Exec.instance.All_Equipment;

            List<IEquipmentObject> allHj = Exec.instance.All_Shelves;

            allObjetcs.ForEach(equipment =>
            {
                if (equipment.type == IEquipmentObject.DDJ && equipment.CanConnect() && false)
                {
                    Point? curPoint = equipment.GetDDJCurPoint();
                    if (curPoint != null)
                    {
                        equipment.point.layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(curPoint.x), Exec.instance.PointMaps)}";
                        //equipment.point.layout_y = $"{Factory<PointMapService>.instance.GetPoint("y", float.Parse(curPoint.z), Exec.instance.PointMaps)}";
                    }
                }
                // 模拟
                if (equipment.id == "aac89211-a3b1-4699-9b13-86485d0de7df")
                {
                    equipment.point.layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(RunningPoints.instance.curPoint.x), Exec.instance.PointMaps)}";
                }
            });

            allHj.ForEach(hj =>
            {
                List<ShelvesPosition> hws = Exec.instance.All_Shelves_Position.FindAll(x => x.layout_id == hj.id);

                int maxLoad = hws.Count();

                int curLoaded = hws.FindAll(x => x.is_loaded).Count();
                //0 - 空载，1 - 1 % 30 %，2 - 30 % 60 %，3 - 60 % 100 %

                float b = (float)curLoaded / (float)maxLoad;

                if (b == 0)
                {
                    hj.cur_loaded = 0;
                }
                else if (b > 0 && b <= 0.3)
                {
                    hj.cur_loaded = 1;
                }
                else if (b > 0.3 && b <= 0.6)
                {
                    hj.cur_loaded = 2;
                }
                else
                {
                    hj.cur_loaded = 3;
                }
            });

            result.AddRange(allObjetcs);
            result.AddRange(allHj);

            return new JsonResult(new
            {
                all_objects = result
            });
        }

        /// <summary>
        /// 手工生成入库任务
        /// </summary>
        /// <param name="startObject"></param>
        /// <param name="endObject"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ManualRkMode([FromQuery] string startObject, [FromQuery] string endObject)
        {
            HttpResult result = new HttpResult()
            {
                code = "200",
                message = "",
                status = true
            };

            // 查找起始对象（输送线对象）
            IEquipmentObject? ssx = Exec.instance.All_Equipment.Find(e => e.id == startObject && e.type == IEquipmentObject.SSX && !string.IsNullOrEmpty(e.IOPoint));
            if (ssx != null)
            {
                IEquipmentObject? ddj = Exec.instance.All_Equipment.Find(e => e.type == IEquipmentObject.DDJ && e.id == ssx.relation_object_id);
                if (ddj != null)
                {
                    ShelvesPosition? hw = Exec.instance.All_Shelves_Position.Find(e => e.relation_object_id == ddj.id && !e.is_loaded && !e.is_locked && e.id == endObject);
                    if (hw != null)
                    {
                        TaskEntity newTask = new TaskEntity();
                        newTask.CreateTime = DateTime.Now;
                        newTask.task_type = rkType;
                        newTask.start_object = ssx.id;
                        newTask.task_status = Exec.wait;
                        newTask.process_object = ddj.id;
                        newTask.end_object = hw.id;
                        hw.is_locked = true;

                        Factory<ShelvesService>.instance.UpdateSheviesPositionLock(hw.id, true);
                        Factory<TaskService>.instance.AddTask(newTask);
                    }
                    else
                    {
                        result.status = false;
                        result.message = "无可用货位信息";
                    }
                }
                else
                {
                    result.status = false;
                    result.message = "无可用堆垛机信息";
                }
            }
            else
            {
                result.status = false;
                result.message = "输送线对象未找到";
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// 手工生成出库任务
        /// </summary>
        /// <param name="startObject"></param>
        /// <param name="endObject"></param>
        /// <returns></returns>
        public JsonResult ManualCkMode([FromQuery] string startObject, [FromQuery] string endObject)
        {
            HttpResult result = new HttpResult()
            {
                code = "200",
                message = "",
                status = true
            };

            // 查找起始对象（货位对象）
            ShelvesPosition? hw = Exec.instance.All_Shelves_Position.Find(e => e.is_loaded && !e.is_locked && e.id == startObject && e.status == 0);
            if (hw != null)
            {
                IEquipmentObject? ddj = Exec.instance.All_Equipment.Find(e => e.type == IEquipmentObject.DDJ && e.id == hw.relation_object_id);
                if (ddj != null)
                {
                    // 找到一个出库用输送线
                    IEquipmentObject? ssx = Exec.instance.All_Equipment.Find(e => e.type == IEquipmentObject.SSX && e.relation_object_id == ddj.id && !string.IsNullOrEmpty(e.IOPoint));
                    if (ssx != null)
                    {
                        TaskEntity newTask = new TaskEntity();
                        newTask.CreateTime = DateTime.Now;
                        newTask.task_type = ckType;
                        newTask.start_object = hw.id;
                        newTask.task_status = Exec.wait;
                        newTask.process_object = ddj.id;
                        newTask.end_object = ssx.id;
                        hw.is_locked = true;

                        Factory<ShelvesService>.instance.UpdateSheviesPositionLock(hw.id, true);
                        Factory<TaskService>.instance.AddTask(newTask);
                    }
                    else
                    {
                        result.status = false;
                        result.message = "出库输送线对象未找到";
                    }
                }
                else
                {
                    result.status = false;
                    result.message = "无可用堆垛机信息";
                }
            }
            else
            {
                result.status = false;
                result.message = "起始货对象未找到";
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// 取消任务-将对应货位锁定信息内存赋值
        /// </summary>
        /// <param name="hwId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CancelTask([FromQuery] string hwId)
        {
            HttpResult result = new HttpResult()
            {
                code = "200",
                message = "",
                status = true
            };

            // 查找被取消的货位对象
            ShelvesPosition? hw = Exec.instance.All_Shelves_Position.Find(e => e.id == hwId);

            if (hw != null)
            {
                hw.is_locked = false;
            }
            else
            {
                result.status = false;
                result.message = "无可用货位信息";
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// 更新货位信息
        /// </summary>
        /// <param name="hwId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UpdateShelvesPosition([FromQuery] string hwId, [FromQuery] int status)
        {
            HttpResult result = new HttpResult()
            {
                code = "200",
                message = "",
                status = true
            };

            // 查找被取消的货位对象
            ShelvesPosition? hw = Exec.instance.All_Shelves_Position.Find(e => e.id == hwId);

            if (hw != null)
            {
                hw.status = status;
            }
            else
            {
                result.status = false;
                result.message = "无可用货位信息";
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// 入库任务自动生成
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AutoMode()
        {
            try
            {
                Exec.instance.All_Equipment.ForEach(x =>
                {
                    if (x.type == IEquipmentObject.SSX && !string.IsNullOrEmpty(x.IOPoint))
                    {
                        // 货位货物是否就绪
                        bool ready = x.GoodIsReady();
                        if (ready || true)
                        {
                            TaskEntity newTask = new TaskEntity();
                            newTask.CreateTime = DateTime.Now;
                            newTask.task_type = rkType;
                            newTask.start_object = x.id;
                            newTask.task_status = Exec.wait;

                            // 生成任务
                            IEquipmentObject? ddj = Exec.instance.All_Equipment.Find(e => e.type == IEquipmentObject.DDJ && e.id == x.relation_object_id);

                            if (ddj != null)
                            {
                                ShelvesPosition? hw = Exec.instance.All_Shelves_Position.Find(e => e.relation_object_id == ddj.id && !e.is_loaded && !e.is_locked && e.status == 0);

                                if (hw != null)
                                {
                                    newTask.process_object = ddj.id;
                                    newTask.end_object = hw.id;
                                    hw.is_locked = true;

                                    Factory<ShelvesService>.instance.UpdateSheviesPositionLock(hw.id, true);
                                    Factory<TaskService>.instance.AddTask(newTask);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                string error = $"自动生成任务异常:{ex.Message}";
                CConsole.WriteLine(error, OutPut.error);
                LLog.log.Error(error);
            }

            return Ok("true");
        }

        /// <summary>
        /// 执行任务
        /// 每五秒钟执行一次任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DealTask()
        {
            try
            {
                // 执行任务
                if (!Exec.instance.isBusy && Exec.instance.isReady)
                {
                    Exec.instance.DealTask();
                }
                else
                {
                    LLog.log.Info("请在WCS中检查设置后，再尝试启动执行机程序...");
                    CConsole.WriteLine("请在WCS中检查设置后，再尝试启动执行机程序...", OutPut.warning);
                }
            }
            catch (Exception ex)
            {
                string error = $"自动执行任务异常:{ex.Message}";
                CConsole.WriteLine(error, OutPut.error);
                LLog.log.Error(error);
            }

            return Ok(true);
        }

        /// <summary>
        /// 查询执行机消息信息
        /// </summary>
        /// <param name="type">INFO/ERROR</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetServerMessages([FromQuery] string type)
        {
            return new JsonResult(MessageContainer.instance.Messages.FindAll(x => x.type == type));
        }

        /// <summary>
        /// 查询可运行设备状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEquipmentStatus()
        {
            List<EquipmentStatus> equipments = new List<EquipmentStatus>();
            // 1、获取所有设备对象
            List<IEquipmentObject> allObjetcs = Exec.instance.All_Equipment;

            // 2、当前设备状态
            allObjetcs.ForEach(x =>
            {
                equipments.Add(new EquipmentStatus()
                {
                    name = x.name,
                    is_busy = x.is_busy,
                    is_connected = x.CanConnect()
                });
            });

            return new JsonResult(new { equipments });
        }

        /// <summary>
        /// 初始化内存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult InitAllData()
        {
            HttpResult result = new HttpResult()
            {
                code = "200",
                message = "",
                status = true
            };

            if (!Exec.instance.isBusy)
            {
                Exec.instance.InitObject();
            }
            else
            {
                result.status = false;
                result.message = "还有执行中的任务";
            }

            return new JsonResult(result);
        }
    }
}
