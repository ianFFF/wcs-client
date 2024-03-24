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
    public class PostController : BaseSimpleCrudController<PostEntity>
    {
        private IPostService _iPostService;
        public PostController(ILogger<PostEntity> logger, IPostService iPostService) : base(logger, iPostService)
        {
            _iPostService = iPostService;
        }

        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="post"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] PostEntity post, [FromQuery] PageParModel page)
        {
            return Result.Success().SetData(await _iPostService.SelctPageList(post, page));
        }



        public override async Task<Result> Add(PostEntity entity)
        {
            return await base.Add(entity);
        }

        public override async Task<Result> Update(PostEntity entity)
        {
            return await base.Update(entity);
        }
    }
}
