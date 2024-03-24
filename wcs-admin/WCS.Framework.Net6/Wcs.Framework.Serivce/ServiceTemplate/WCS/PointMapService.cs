using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class PointMapService : BaseService<PointMapEntity>, IPointMapService
    {
        public PointMapService(IRepository<PointMapEntity> repository) : base(repository)
        {
        }
    }
}
