using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class PostService : BaseService<PostEntity>, IPostService
    {
        public async Task<PageModel<List<PostEntity>>> SelctPageList(PostEntity post, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                    .WhereIF(!string.IsNullOrEmpty(post.PostName), u => u.PostName.Contains(post.PostName))
                        .WhereIF(!string.IsNullOrEmpty(post.PostCode), u => u.PostCode.Contains(post.PostCode))
                     .WhereIF(post.IsDeleted.IsNotNull(), u => u.IsDeleted == post.IsDeleted)

                    .OrderBy(u => u.OrderNum, OrderByType.Desc)
                    .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<PostEntity>>(data, total);
        }
    }
}
