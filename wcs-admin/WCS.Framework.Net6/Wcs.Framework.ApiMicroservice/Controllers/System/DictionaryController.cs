using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
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
    public class DictionaryController :BaseSimpleCrudController<DictionaryEntity>
    {
        private IDictionaryService _iDictionaryService;
        public DictionaryController(ILogger<DictionaryEntity> logger, IDictionaryService iDictionaryService):base(logger, iDictionaryService)
        {
            _iDictionaryService = iDictionaryService;
        }

        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] DictionaryEntity dic, [FromQuery] PageParModel page)
        {
            return Result.Success().SetData(await _iDictionaryService.SelctPageList(dic, page));
        }
    }
}
