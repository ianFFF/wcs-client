using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
   public partial interface IPostService:IBaseService<PostEntity>
    {
        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="post"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<PageModel<List<PostEntity>>> SelctPageList(PostEntity post, PageParModel page);
    }
}
