using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class RoleDeptService : BaseService<RoleDeptEntity>, IRoleDeptService
    {
        public RoleDeptService(IRepository<RoleDeptEntity> repository) : base(repository)
        {
        }
    }
}
