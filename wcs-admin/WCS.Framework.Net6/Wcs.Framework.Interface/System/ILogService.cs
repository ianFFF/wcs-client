using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface ILogService 
    {
        Task<List<long>> AddListTest(List<LogEntity> logEntities);
        Task<List<LogEntity>> GetListTest();
    }
}
