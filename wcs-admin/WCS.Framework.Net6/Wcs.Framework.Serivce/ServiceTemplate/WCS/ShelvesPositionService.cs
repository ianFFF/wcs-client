using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class ShelvesPositionService : BaseService<ShelvesPositionEntity>, IShelvesPositionService
    {
        public ShelvesPositionService(IRepository<ShelvesPositionEntity> repository) : base(repository)
        {
        }
    }
}
