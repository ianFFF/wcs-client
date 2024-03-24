using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.WebCore;
using Wcs.Framework.WebCore.AttributeExtend;
using Wcs.Framework.WebCore.AuthorizationPolicy;

namespace Wcs.Framework.ApiMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OperationLogController : BaseSimpleCrudController<OperationLogEntity>
    {
        private IOperationLogService _iOperationLogService;
        public OperationLogController(ILogger<OperationLogEntity> logger, IOperationLogService iOperationLogService) : base(logger, iOperationLogService)
        {
            _iOperationLogService = iOperationLogService;
        }
        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="operationLog"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] OperationLogEntity operationLog, [FromQuery] PageParModel page)
        {
            return Result.Success().SetData(await _iOperationLogService.SelctPageList(operationLog, page));
        }


    }
}
