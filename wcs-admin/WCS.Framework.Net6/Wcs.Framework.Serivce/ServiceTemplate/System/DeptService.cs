using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class DeptService : BaseService<DeptEntity>, IDeptService
    {
        public DeptService(IRepository<DeptEntity> repository) : base(repository)
        {
        }
    }
}
