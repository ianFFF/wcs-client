﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Language;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Model.Query;
using Wcs.Framework.Repository;
using Wcs.Framework.WebCore.AttributeExtend;

namespace Wcs.Framework.ApiMicroservice.Controllers
{
    /// <summary>
    /// Json To Sql 类比模式，通用模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ApiController]
    public class BaseSimpleRdController<T> : BaseExcelController<T> where T : class, new()
    {
        protected readonly ILogger<T> _logger;
        protected IBaseService<T> _baseService;
        public BaseSimpleRdController(ILogger<T> logger, IBaseService<T> iBaseService):base(iBaseService._repository)
        {
            _logger = logger;
            _baseService = iBaseService;
        }

        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public virtual async Task<Result> GetById([FromRoute]long id)
        {
            return Result.Success().SetData(await _repository.GetByIdAsync(id));
        }

        /// <summary>
        /// 全部列表查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Result> GetList()
        {
            return Result.Success().SetData(await _repository.GetListAsync());
        }

        /// <summary>
        /// 列表删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<Result> DelList(List<long> ids)
        {
            return Result.Success().SetStatus(await _repository.DeleteByIdsAsync(ids.ToDynamicArray()));
        }
    }
}
