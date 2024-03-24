using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;

namespace Wcs.Framework.Model.Models
{
    [SugarTable("wcs_equipment_logs")]
    public partial class EquipmentLogsEntity : BaseV2ModelEntity
    {
        [SugarColumn(ColumnName = "equipment_id", Length = 36)]
        public string? equipment_id { get; set; }

        [SugarColumn(ColumnName = "logs_interval")]
        public int? logs_interval { get; set; }

        [SugarColumn(ColumnName = "logs_interval_type")]
        public int? logs_interval_type { get; set; }

        [SugarColumn(ColumnName = "logs_is_record")]
        public bool? logs_is_record { get; set; }
    }
}
