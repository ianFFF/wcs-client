using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
   public partial interface ISpuService:IBaseService<SpuEntity>
    {
        /// <summary>
        /// 动态条件分页查询
        /// </summary>
        /// <param name="eneity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<PageModel<List<SpuEntity>>> SelctPageList(SpuEntity entity, PageParModel page);
    }
}
