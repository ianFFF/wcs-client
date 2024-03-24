using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;

namespace Wcs.Framework.Model.Models
{
    [SugarTable("wcs_equipment_alarm")]
    public partial class EquipmentAlarmEntity : BaseV2ModelEntity
    {

        [SugarColumn(ColumnName = "equipment_type_id", Length = 36)]
        public string? equipment_type_id { get; set; }

        [SugarColumn(ColumnName = "equipment_id", Length = 36)]
        public string? equipment_id { get; set; }

        [SugarColumn(ColumnName = "alarm_level", Length = 36)]
        public string? alarm_level { get; set; }

        [SugarColumn(ColumnName = "alarm_code", Length = 50)]
        public string? alarm_code { get; set; }

        [SugarColumn(ColumnName = "alarm_remark", Length = 200)]
        public string? alarm_remark { get; set; }
    }
}
