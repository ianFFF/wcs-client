using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class UserService : BaseService<UserEntity>, IUserService
    {
        public UserService(IRepository<UserEntity> repository) : base(repository)
        {
        }
    }
}
