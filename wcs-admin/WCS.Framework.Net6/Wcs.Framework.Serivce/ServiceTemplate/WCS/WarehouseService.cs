using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class WarehouseService : BaseService<WarehouseEntity>, IWarehouseService
    {
        public WarehouseService(IRepository<WarehouseEntity> repository) : base(repository)
        {
        }
    }
}
