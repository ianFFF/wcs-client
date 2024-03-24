using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IShelvesService : IBaseService<ShelvesEntity>
    {
        Task<PageModel<List<ShelvesDto>>> SelctPageList(ShelvesEntity entity, PageParModel page);

        Task<bool> AddEntity(ShelvesEntity entity);

        Task<ShelvesEntity> GetDetailById(string id);

        Task<List<ShelvesEntity>> SelctListForManual(string equipmentId);
    }
}
