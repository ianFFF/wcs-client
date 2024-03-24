using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
   public partial interface IConfigService:IBaseService<ConfigEntity>
    {
        Task<PageModel<List<ConfigEntity>>> SelctPageList(ConfigEntity config, PageParModel page);
    }
}
