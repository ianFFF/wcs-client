using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IEquipmentService : IBaseService<EquipmentEntity>
    {
        Task<PageModel<List<EquipmentDto>>> SelctPageList(EquipmentEntity entity, PageParModel page);

        Task<bool> AddEntity(EquipmentEntity entity);

        Task<EquipmentEntity> GetDetailById(string id);

        Task<PageModel<List<EquipmentEntity>>> SelctList(string type);

        Task<List<int?>> GetAllPointXReal();

        Task<List<int?>> GetAllPointYReal();

        Task<List<EquipmentEntity>> SelctListForManual(string type, string relation_object_id);
    }
}
