using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class SpecsService : BaseService<SpecsEntity>, ISpecsService
    {
        public SpecsService(IRepository<SpecsEntity> repository) : base(repository)
        {
        }
    }
}
