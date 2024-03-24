using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class OperationLogService : BaseService<OperationLogEntity>, IOperationLogService
    {
        public OperationLogService(IRepository<OperationLogEntity> repository) : base(repository)
        {
        }
    }
}
