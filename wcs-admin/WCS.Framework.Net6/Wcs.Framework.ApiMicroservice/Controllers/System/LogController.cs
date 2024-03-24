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
    public class LogController : ControllerBase
    {
        private ILogService _iLogService;
        //大量日志，将采用自动分表形式，默认1年分一次表
        public LogController(ILogger<LogEntity> logger, ILogService iLogService)
        {
            _iLogService = iLogService;
        }

        /// <summary>
        /// 自动分表，日志添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Add()
        {
            Random random = new Random();
            var logList = new List<LogEntity>() {
                new LogEntity() { CreateTime = Convert.ToDateTime("2019-12-1"), Message = "jack"+random.Next() } ,
                new LogEntity() { CreateTime = Convert.ToDateTime("2022-02-1"), Message = "jack"+random.Next() },
                new LogEntity() { CreateTime = Convert.ToDateTime("2020-02-1"), Message = "jack"+random.Next() },
                new LogEntity() { CreateTime = Convert.ToDateTime("2021-12-1"), Message = "jack"+random.Next() } };
            return Result.Success().SetData(await _iLogService.AddListTest(logList));
        }
  
        /// <summary>
        /// 查询近20年与21年的日志表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetList()
        {
            return Result.Success().SetData(await _iLogService.GetListTest());
        }
    }
}
