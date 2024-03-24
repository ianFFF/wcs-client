using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IEquipmentRecordsService : IBaseService<EquipmentRecordsEntity>
    {
        Task<PageModel<List<EquipmentRecordsDto>>> SelctPageList(EquipmentRecordsDto entity, PageParModel page);
        Task<List<EquipmentRecordsExcelDto>> ExportList(EquipmentRecordsDto entity);
    }
}
