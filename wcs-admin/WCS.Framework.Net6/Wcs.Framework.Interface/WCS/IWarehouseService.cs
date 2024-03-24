using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IWarehouseService : IBaseService<WarehouseEntity>
    {
        Task<PageModel<List<WarehouseEntity>>> SelctList();
    }
}
