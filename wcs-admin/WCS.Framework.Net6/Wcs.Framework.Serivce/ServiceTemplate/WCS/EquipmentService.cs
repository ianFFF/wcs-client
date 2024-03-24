using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentService : BaseService<EquipmentEntity>, IEquipmentService
    {
        public EquipmentService(IRepository<EquipmentEntity> repository) : base(repository)
        {
        }
    }
}
