using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class LoginLogService : BaseService<LoginLogEntity>, ILoginLogService
    {
        public LoginLogService(IRepository<LoginLogEntity> repository) : base(repository)
        {
        }
    }
}
