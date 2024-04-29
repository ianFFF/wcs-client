using System;
using wcs.core.common;
using wcs.core.model;
using SqlSugar;

namespace wcs.core.service
{
    public class TaskService
    {
        /// <summary>
        /// 获取一个最新的等待中任务
        /// </summary>
        /// <returns></returns>
        public TaskEntity GetTask()
        {
            return DbContext.db.Queryable<TaskEntity>()
                .Where(x => x.IsDeleted == false && x.task_status == "等待")
                .OrderBy(x => x.CreateTime, OrderByType.Asc)
                .First();
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="error"></param>
        public void UpdateTaskStatus(string id, string status, string error)
        {
            // 任务状态; 等待、运行中、结束、异常中止
            DbContext.db.Updateable<TaskEntity>()
                .SetColumns(x => x.task_status == status)
                .SetColumns(x => x.task_error == error)
                .SetColumns(x => x.ModifyTime == DateTime.Now)
                .SetColumnsIF(status == Exec.complete, x => x.CompleteTime == DateTime.Now)
                .Where(x => x.Id == id)
                .ExecuteCommand();
        }

        /// <summary>
        /// 插入一个任务
        /// </summary>
        /// <param name="model"></param>
        public void AddTask(TaskEntity model)
        {
            int s = getTaskTotal();
            model.task_code = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{s}";
            model.loader_code = $"TP00{s}";

            DbContext.db.Insertable<TaskEntity>(model).ExecuteCommand();
        }

        /// <summary>
        /// 任务总数
        /// </summary>
        /// <returns></returns>
        private int getTaskTotal()
        {
            return DbContext.db.Queryable<TaskEntity>().Where(x => x.IsDeleted == false).Count();
        }
    }
}

