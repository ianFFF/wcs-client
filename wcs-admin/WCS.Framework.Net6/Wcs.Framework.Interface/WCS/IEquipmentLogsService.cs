using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IEquipmentLogsService : IBaseService<EquipmentLogsEntity>
    {
        Task<PageModel<List<EquipmentLogsDto>>> SelctPageList(EquipmentLogsDto entity, PageParModel page);

        Task<bool> AddEntity(EquipmentLogsEntity entity);

        Task<EquipmentLogsEntity> GetDetailById(string id);
    }
}
