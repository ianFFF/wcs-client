using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 任务类型
    ///</summary>
    [SugarTable("wcs_task_type")]
    public partial class TaskTypeEntity : BaseV2ModelEntity
    {
        /// <summary>
        /// 任务类型 
        ///</summary>
        [SugarColumn(ColumnName = "task_type", Length = 50)]
        public string? task_type { get; set; }
    }
}
