using System;
using SqlSugar;

namespace wcs.core.model
{
    /// <summary>
    /// 任务主表
    /// </summary>
    [SugarTable("wcs_task")]
    public partial class TaskEntity : BaseModel
    {
        /// <summary>
        /// 任务类型 
        ///</summary>
        [SugarColumn(ColumnName = "task_type", Length = 36)]
        public string? task_type { get; set; }

        /// <summary>
        /// 起始对象
        /// </summary>
        [SugarColumn(ColumnName = "start_object", Length = 36)]
        public string? start_object { get; set; }

        /// <summary>
        /// 任务执行对象
        /// </summary>
        [SugarColumn(ColumnName = "process_object", Length = 36)]
        public string? process_object { get; set; }

        /// <summary>
        /// 重点对象
        /// </summary>
        [SugarColumn(ColumnName = "end_object", Length = 36)]
        public string? end_object { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [SugarColumn(ColumnName = "task_status", Length = 20)]
        public string? task_status { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [SugarColumn(ColumnName = "task_error", Length = 50)]
        public string? task_error { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [SugarColumn(ColumnName = "task_code", Length = 50)]
        public string? task_code { get; set; }

        /// <summary>
        /// 托盘码
        /// </summary>
        [SugarColumn(ColumnName = "loader_code", Length = 50)]
        public string? loader_code { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(ColumnName = "CompleteTime")]
        public DateTime? CompleteTime { get; set; }
    }
}

