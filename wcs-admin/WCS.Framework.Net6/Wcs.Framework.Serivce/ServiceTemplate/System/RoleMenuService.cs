using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class RoleMenuService : BaseService<RoleMenuEntity>, IRoleMenuService
    {
        public RoleMenuService(IRepository<RoleMenuEntity> repository) : base(repository)
        {
        }
    }
}
