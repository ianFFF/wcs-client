using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IEquipmentAlarmService : IBaseService<EquipmentAlarmEntity>
    {
        Task<PageModel<List<EquipmentAlarmDto>>> SelctPageList(EquipmentAlarmDto entity, PageParModel page);

        Task<bool> AddEntity(EquipmentAlarmEntity entity);

        Task<EquipmentAlarmEntity> GetDetailById(string id);

        Task<List<EquipmentAlarmDto>> GetAlarmCode();
    }
}
