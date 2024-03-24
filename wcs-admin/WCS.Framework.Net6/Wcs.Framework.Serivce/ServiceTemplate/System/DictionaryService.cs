using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class DictionaryService : BaseService<DictionaryEntity>, IDictionaryService
    {
        public DictionaryService(IRepository<DictionaryEntity> repository) : base(repository)
        {
        }
    }
}
