using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class UserPostService : BaseService<UserPostEntity>, IUserPostService
    {
        public UserPostService(IRepository<UserPostEntity> repository) : base(repository)
        {
        }
    }
}
