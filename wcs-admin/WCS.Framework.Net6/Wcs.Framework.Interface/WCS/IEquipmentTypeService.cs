using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.DTOModel;

namespace Wcs.Framework.Interface
{
    public partial interface IEquipmentTypeService : IBaseService<EquipmentTypeEntity>
    {
        Task<PageModel<List<EquipmentTypeEntity>>> SelctList();
        Task<List<AlarmTypeVo>> GetAlarmType();
    }
}
