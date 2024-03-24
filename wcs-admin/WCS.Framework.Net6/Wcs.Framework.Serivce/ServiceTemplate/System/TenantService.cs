using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class TenantService : BaseService<TenantEntity>, ITenantService
    {
        public TenantService(IRepository<TenantEntity> repository) : base(repository)
        {
        }
    }
}
