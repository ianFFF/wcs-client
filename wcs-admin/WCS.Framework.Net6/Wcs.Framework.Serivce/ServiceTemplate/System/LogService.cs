using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class LogService : BaseService<LogEntity>, ILogService
    {
        public LogService(IRepository<LogEntity> repository) : base(repository)
        {
        }
    }
}
