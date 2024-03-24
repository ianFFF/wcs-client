using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface IPointMapService : IBaseService<PointMapEntity>
    {
        Task<bool> AddEntity(PointMapEntity entity);

        Task<bool> ClearTable();
    }
}
