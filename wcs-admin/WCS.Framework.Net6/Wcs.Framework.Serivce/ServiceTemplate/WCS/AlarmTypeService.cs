using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class AlarmLevelService : BaseService<AlarmLevelEntity>, IAlarmLevelService
    {
        public AlarmLevelService(IRepository<AlarmLevelEntity> repository) : base(repository)
        {
        }
    }
}
