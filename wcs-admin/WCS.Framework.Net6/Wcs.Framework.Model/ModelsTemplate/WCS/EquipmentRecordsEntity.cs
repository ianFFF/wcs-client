using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;

namespace Wcs.Framework.Model.Models
{
    [SugarTable("wcs_equipment_records")]
    public partial class EquipmentRecordsEntity : BaseV2ModelEntity
    {
        [SugarColumn(ColumnName = "alarm_level", Length = 36)]
        public string? alarm_level { get; set; }

        [SugarColumn(ColumnName = "alarm_type", Length = 50)]
        public string? alarm_type { get; set; }

        [SugarColumn(ColumnName = "alarm_code", Length = 20)]
        public string? alarm_code { get; set; }

        [SugarColumn(ColumnName = "message", Length = 100)]
        public string? message { get; set; }

        [SugarColumn(ColumnName = "task_id", Length = 36)]
        public string? task_id { get; set; }
    }
}
