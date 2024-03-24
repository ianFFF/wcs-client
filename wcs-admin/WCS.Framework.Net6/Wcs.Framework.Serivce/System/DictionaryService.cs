﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class DictionaryService : BaseService<DictionaryEntity>, IDictionaryService
    {
        public async Task<PageModel<List<DictionaryEntity>>> SelctPageList(DictionaryEntity dic, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                    .WhereIF(!string.IsNullOrEmpty(dic.DictName), u => u.DictName.Contains(dic.DictName))
                     .WhereIF(!string.IsNullOrEmpty(dic.DictType), u => u.DictType.Contains(dic.DictType))
                    .WhereIF(page.StartTime.IsNotNull() && page.EndTime.IsNotNull(), u => u.CreateTime >= page.StartTime && u.CreateTime <= page.EndTime)
                     .Where(u => u.IsDeleted == false)
                    .OrderBy(u => u.OrderNum, OrderByType.Desc)
                    .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<DictionaryEntity>>(data, total);
        }

    }
}
