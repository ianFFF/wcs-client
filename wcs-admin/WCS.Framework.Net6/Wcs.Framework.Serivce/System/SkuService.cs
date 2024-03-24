using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class SkuService : BaseService<SkuEntity>, ISkuService
    {
        public async Task<PageModel<List<SkuEntity>>> SelctPageList(SkuEntity enetity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable

                    .WhereIF(page.StartTime is not null && page.EndTime is not null, u => u.CreateTime >= page.StartTime && u.CreateTime <= page.EndTime)
                     .WhereIF(enetity.IsDeleted is not null, u => u.IsDeleted == enetity.IsDeleted)
                    .OrderBy(u => u.CreateTime, OrderByType.Desc)
                    .ToPageListAsync(page.PageNum, page.PageSize, total);
            return new PageModel<List<SkuEntity>>(data, total);
        }
    }
}
