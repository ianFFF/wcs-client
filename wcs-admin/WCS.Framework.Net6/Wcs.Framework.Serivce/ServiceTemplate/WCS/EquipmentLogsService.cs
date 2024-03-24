using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentLogsService : BaseService<EquipmentLogsEntity>, IEquipmentLogsService
    {
        public EquipmentLogsService(IRepository<EquipmentLogsEntity> repository) : base(repository)
        {
        }
    }
}
