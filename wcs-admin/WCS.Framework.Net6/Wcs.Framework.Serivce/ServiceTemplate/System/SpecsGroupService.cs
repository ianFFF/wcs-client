using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class SpecsGroupService : BaseService<SpecsGroupEntity>, ISpecsGroupService
    {
        public SpecsGroupService(IRepository<SpecsGroupEntity> repository) : base(repository)
        {
        }
    }
}
