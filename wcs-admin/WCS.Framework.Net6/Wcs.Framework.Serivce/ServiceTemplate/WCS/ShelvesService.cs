using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class ShelvesService : BaseService<ShelvesEntity>, IShelvesService
    {
        public ShelvesService(IRepository<ShelvesEntity> repository) : base(repository)
        {
        }
    }
}
