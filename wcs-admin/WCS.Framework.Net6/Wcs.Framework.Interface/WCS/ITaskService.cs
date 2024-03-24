using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public partial interface ITaskService : IBaseService<TaskEntity>
    {
        Task<PageModel<List<TaskDto>>> SelctPageList(TaskEntity entity, PageParModel page);
        Task<TaskEntity> GetDetailById(string id);
        Task<bool> DeleteById(string id);
        Task<TaskStatisticsDto> TaskStatistics();
        Task<bool> InitTask();
        Task<List<TaskDto>> GetTop3Task();
    }
}
