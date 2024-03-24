using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class SkuService : BaseService<SkuEntity>, ISkuService
    {
        public SkuService(IRepository<SkuEntity> repository) : base(repository)
        {
        }
    }
}
