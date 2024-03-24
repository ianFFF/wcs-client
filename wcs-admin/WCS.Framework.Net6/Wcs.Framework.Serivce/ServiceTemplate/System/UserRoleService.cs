using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class UserRoleService : BaseService<UserRoleEntity>, IUserRoleService
    {
        public UserRoleService(IRepository<UserRoleEntity> repository) : base(repository)
        {
        }
    }
}
