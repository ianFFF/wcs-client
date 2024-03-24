using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class PostService : BaseService<PostEntity>, IPostService
    {
        public PostService(IRepository<PostEntity> repository) : base(repository)
        {
        }
    }
}
