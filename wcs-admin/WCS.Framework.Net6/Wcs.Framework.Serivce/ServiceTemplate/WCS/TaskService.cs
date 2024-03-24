using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class TaskService : BaseService<TaskEntity>, ITaskService
    {
        public TaskService(IRepository<TaskEntity> repository) : base(repository)
        {
        }
    }
}
