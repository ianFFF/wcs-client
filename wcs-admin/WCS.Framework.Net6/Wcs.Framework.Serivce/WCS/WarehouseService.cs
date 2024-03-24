using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class WarehouseService : BaseService<WarehouseEntity>, IWarehouseService
    {
        /// <summary>
        /// 获取库区信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<WarehouseEntity>>> SelctList()
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CreateTime, OrderByType.Desc)
                .ToListAsync();

            return new PageModel<List<WarehouseEntity>>(data, total);
        }

    }
}
