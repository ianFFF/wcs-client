using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class CategoryService : BaseService<CategoryEntity>, ICategoryService
    {
        public CategoryService(IRepository<CategoryEntity> repository) : base(repository)
        {
        }
    }
}
