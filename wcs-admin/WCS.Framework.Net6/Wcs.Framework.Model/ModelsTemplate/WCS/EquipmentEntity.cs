using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;

namespace Wcs.Framework.Model.Models
{
    [SugarTable("wcs_equipment")]
    public partial class EquipmentEntity : BaseV2ModelEntity
    {
        [SugarColumn(ColumnName = "equipment_type", Length = 36)]
        public string? equipment_type { get; set; }

        [SugarColumn(ColumnName = "equipment_name", Length = 50)]
        public string? equipment_name { get; set; }

        [SugarColumn(ColumnName = "equipment_supplier", Length = 100)]
        public string? equipment_supplier { get; set; }

        [SugarColumn(ColumnName = "upper_system_id", Length = 50)]
        public string? upper_system_id { get; set; }

        [SugarColumn(ColumnName = "ip", Length = 20)]
        public string? ip { get; set; }

        [SugarColumn(ColumnName = "port", Length = 20)]
        public string? port { get; set; }

        [SugarColumn(ColumnName = "warehouse_area", Length = 36)]
        public string? warehouse_area { get; set; }

        [SugarColumn(ColumnName = "relation_object_id", Length = 36)]
        public string? relation_object_id { get; set; }

        [SugarColumn(ColumnName = "point_x")]
        public int? point_x { get; set; }

        [SugarColumn(ColumnName = "point_y")]
        public int? point_y { get; set; }

        [SugarColumn(ColumnName = "point_z")]
        public int? point_z { get; set; }

        /// <summary>
        /// 监听PLC输出点位
        /// </summary>
        [SugarColumn(ColumnName = "io_point", Length = 50)]
        public string? io_point { get; set; }
    }
}
