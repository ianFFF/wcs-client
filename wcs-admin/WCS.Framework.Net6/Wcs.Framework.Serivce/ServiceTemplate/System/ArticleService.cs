using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class ArticleService : BaseService<ArticleEntity>, IArticleService
    {
        public ArticleService(IRepository<ArticleEntity> repository) : base(repository)
        {
        }
    }
}
