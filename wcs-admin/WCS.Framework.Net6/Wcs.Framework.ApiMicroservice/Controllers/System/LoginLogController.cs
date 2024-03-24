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
    public class LoginLogController : BaseSimpleCrudController<LoginLogEntity>
    {
        private ILoginLogService _iLoginLogService;
        public LoginLogController(ILogger<LoginLogEntity> logger, ILoginLogService iLoginLogService) : base(logger, iLoginLogService)
        {
            _iLoginLogService = iLoginLogService;
        }

        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="loginLog"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] LoginLogEntity loginLog, [FromQuery] PageParModel page)
        {
            return Result.Success().SetData(await _iLoginLogService.SelctPageList(loginLog, page));
        }

    }
}
