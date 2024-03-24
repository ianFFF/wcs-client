using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class RoleService : BaseService<RoleEntity>, IRoleService
    {
        public RoleService(IRepository<RoleEntity> repository) : base(repository)
        {
        }
    }
}
