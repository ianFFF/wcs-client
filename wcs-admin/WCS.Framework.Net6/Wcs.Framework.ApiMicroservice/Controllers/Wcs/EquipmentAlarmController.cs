using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
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
    public class EquipmentAlarmController : BaseSimpleCrudController<EquipmentAlarmEntity>
    {
        private readonly IEquipmentAlarmService _iEquipmentAlarmService;
        private readonly IAlarmLevelService _iAlarmTypeService;
        private readonly IMapper _mapper;

        public EquipmentAlarmController(
            ILogger<EquipmentAlarmEntity> logger,
            IEquipmentAlarmService iEquipmentAlarmService,
            IAlarmLevelService iAlarmTypeService,
            IMapper mapper
        ) : base(logger, iEquipmentAlarmService)
        {
            this._mapper = mapper;
            this._iEquipmentAlarmService = iEquipmentAlarmService;
            this._iAlarmTypeService = iAlarmTypeService;
        }

        /// <summary>
        /// 获取所有异常类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetAllAlarm()
        {
            var alarmData = await _iAlarmTypeService.SelctList();

            return Result.Success().SetData(
                new
                {
                    alarms = _mapper.Map<List<AlarmLevelVo>>(alarmData.Data)
                }
            );
        }

        /// <summary>
        /// 获取设备异常配置列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] EquipmentAlarmDto entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iEquipmentAlarmService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<Result> Add(EquipmentAlarmEntity entity)
        {
            return Result.Success().SetData(await _iEquipmentAlarmService.AddEntity(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> Delete(List<string> ids)
        {
            return Result.Success().SetStatus(await _repository.DeleteByIdsAsync(ids.ToArray()));
        }

        /// <summary>
        /// 单个详情查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetDetailById([FromQuery] string id)
        {
            return Result.Success().SetData(await _iEquipmentAlarmService.GetDetailById(id));
        }
    }
}
