using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class ConfigService : BaseService<ConfigEntity>, IConfigService
    {
        public ConfigService(IRepository<ConfigEntity> repository) : base(repository)
        {
        }
    }
}
