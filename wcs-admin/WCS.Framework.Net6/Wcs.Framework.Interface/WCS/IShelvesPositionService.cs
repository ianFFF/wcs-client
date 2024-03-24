using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using System.Data;

namespace Wcs.Framework.Interface
{
    public partial interface IShelvesPositionService : IBaseService<ShelvesPositionEntity>
    {
        Task<bool> AddEntity(ShelvesPositionEntity entity);
        Task<bool> AddEntityByShelves(ShelvesEntity entity);
        Task<bool> UpdateEntityByShelves(ShelvesEntity entity);
        Task<bool> DeleteEntityByShelves(ShelvesEntity entity);
        Task<List<int?>> GetAllPointXReal();
        Task<List<int?>> GetAllPointYReal();
        //Task<List<ShelvesPositionEntity>> GetAllPositionByShelvesId(string id);
        Task<DataTable> GetAllPositionByShelvesId(string id);
        Task<bool> UpdateEntity(ShelvesPositionEntity entity);
        Task<bool> InitLockedLoaded();
    }
}
