using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 异常等级表
    ///</summary>
    [SugarTable("wcs_alarm_level")]
    public partial class AlarmLevelEntity : BaseV2ModelEntity
    {
        /// <summary>
        /// 异常等级 
        ///</summary>
        [SugarColumn(ColumnName = "alarm_level", Length = 50)]
        public string? alarm_level { get; set; }
    }
}
