using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Const;
using Wcs.Framework.Common.Enum;
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
    public class EquipmentRecordsController : BaseSimpleCrudController<EquipmentRecordsEntity>
    {
        private readonly IEquipmentRecordsService _iEquipmentRecordsService;
        private readonly IEquipmentTypeService _iEquipmentTypeService;
        private readonly IEquipmentAlarmService _iEquipmentAlarmService;
        private readonly IMapper _mapper;

        public EquipmentRecordsController(
            ILogger<EquipmentRecordsEntity> logger,
            IEquipmentRecordsService iEquipmentRecordsService,
            IEquipmentTypeService iEquipmentTypeService,
            IEquipmentAlarmService iEquipmentAlarmService,
            IMapper mapper
        ) : base(logger, iEquipmentRecordsService)
        {
            this._mapper = mapper;
            this._iEquipmentRecordsService = iEquipmentRecordsService;
            this._iEquipmentTypeService = iEquipmentTypeService;
            this._iEquipmentAlarmService = iEquipmentAlarmService;
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] EquipmentRecordsDto entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iEquipmentRecordsService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExportExcel(EquipmentRecordsDto entity)
        {
            var data = await _iEquipmentRecordsService.ExportList(entity);
            var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}日志导出";
            var path = ExcelHelper.ExportExcel(data, fileName, Path.Combine(PathConst.wwwroot, PathEnum.Temp.ToString()));
            var file = System.IO.File.OpenRead(path);
            return File(file, "text/plain", $"{fileName}.xlsx");
        }

        /// <summary>
        /// 根据设备类型获取所有设备异常类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetAlarmType()
        {
            var data = await _iEquipmentTypeService.GetAlarmType();

            return Result.Success().SetData(data);
        }

        /// <summary>
        /// 获取所有异常编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetAlarmCode()
        {
            var data = await _iEquipmentAlarmService.GetAlarmCode();

            return Result.Success().SetData(data);
        }
    }
}
