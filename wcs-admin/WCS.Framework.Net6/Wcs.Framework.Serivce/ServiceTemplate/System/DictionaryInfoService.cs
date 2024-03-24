using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class DictionaryInfoService : BaseService<DictionaryInfoEntity>, IDictionaryInfoService
    {
        public DictionaryInfoService(IRepository<DictionaryInfoEntity> repository) : base(repository)
        {
        }
    }
}
