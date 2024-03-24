using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentRecordsService : BaseService<EquipmentRecordsEntity>, IEquipmentRecordsService
    {
        public EquipmentRecordsService(IRepository<EquipmentRecordsEntity> repository) : base(repository)
        {
        }
    }
}
