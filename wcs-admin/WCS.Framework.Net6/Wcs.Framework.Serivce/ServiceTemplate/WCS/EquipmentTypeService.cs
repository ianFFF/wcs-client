using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentTypeService : BaseService<EquipmentTypeEntity>, IEquipmentTypeService
    {
        public EquipmentTypeService(IRepository<EquipmentTypeEntity> repository) : base(repository)
        {
        }
    }
}
