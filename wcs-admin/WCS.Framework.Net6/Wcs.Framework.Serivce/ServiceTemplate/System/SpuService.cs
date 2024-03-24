using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class SpuService : BaseService<SpuEntity>, ISpuService
    {
        public SpuService(IRepository<SpuEntity> repository) : base(repository)
        {
        }
    }
}
