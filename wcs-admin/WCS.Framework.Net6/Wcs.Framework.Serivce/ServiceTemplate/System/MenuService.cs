using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class MenuService : BaseService<MenuEntity>, IMenuService
    {
        public MenuService(IRepository<MenuEntity> repository) : base(repository)
        {
        }
    }
}
