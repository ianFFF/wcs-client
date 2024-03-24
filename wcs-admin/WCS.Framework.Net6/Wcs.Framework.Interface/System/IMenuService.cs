using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
   public partial interface IMenuService:IBaseService<MenuEntity>
    {
        Task<List<MenuEntity>> GetMenuTreeAsync();
        Task<List<MenuEntity>> SelctGetList(MenuEntity menu);

        /// <summary>
        /// 获取该角色id下的所有菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<MenuEntity>> GetListByRoleId(long roleId);
    }
}
