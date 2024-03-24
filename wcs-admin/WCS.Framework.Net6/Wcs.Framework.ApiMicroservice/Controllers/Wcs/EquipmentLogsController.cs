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
    public class EquipmentLogsController : BaseSimpleCrudController<EquipmentLogsEntity>
    {
        private readonly IEquipmentLogsService _iEquipmentLogsService;
        private readonly IMapper _mapper;

        public EquipmentLogsController(
            ILogger<EquipmentLogsEntity> logger,
            IEquipmentLogsService iEquipmentLogsService,
            IMapper mapper
        ) : base(logger, iEquipmentLogsService)
        {
            this._mapper = mapper;
            this._iEquipmentLogsService = iEquipmentLogsService;
        }

        /// <summary>
        /// 获取设备日志配置列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] EquipmentLogsDto entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iEquipmentLogsService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<Result> Add(EquipmentLogsEntity entity)
        {
            return Result.Success().SetData(await _iEquipmentLogsService.AddEntity(entity));
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
            return Result.Success().SetData(await _iEquipmentLogsService.GetDetailById(id));
        }
    }
}
