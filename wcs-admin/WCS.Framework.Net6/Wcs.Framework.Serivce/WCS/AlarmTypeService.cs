using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class AlarmLevelService : BaseService<AlarmLevelEntity>, IAlarmLevelService
    {
        /// <summary>
        /// 获取异常等级信息
        /// </summary>
        /// <returns></returns>
        public async Task<PageModel<List<AlarmLevelEntity>>> SelctList()
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CreateTime, OrderByType.Desc)
                .ToListAsync();

            return new PageModel<List<AlarmLevelEntity>>(data, total);
        }
    }
}
