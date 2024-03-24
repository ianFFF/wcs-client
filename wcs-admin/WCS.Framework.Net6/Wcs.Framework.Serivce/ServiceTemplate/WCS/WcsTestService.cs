using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class WcsTestService : BaseService<WcstestEntity>, IWcsTestService
    {
        public WcsTestService(IRepository<WcstestEntity> repository) : base(repository)
        {
        }
    }
}
