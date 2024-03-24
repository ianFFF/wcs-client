using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;

namespace Wcs.Framework.Model.Models
{
    [SugarTable("wcs_shelves")]
    public partial class ShelvesEntity : BaseV2ModelEntity
    {
        [SugarColumn(ColumnName = "shelves_code", Length = 20)]
        public string? shelves_code { get; set; }

        [SugarColumn(ColumnName = "shelves_cols_num")]
        public int? shelves_cols_num { get; set; }

        [SugarColumn(ColumnName = "shelves_rows_num")]
        public int? shelves_rows_num { get; set; }

        [SugarColumn(ColumnName = "shelves_deep_num")]
        public int? shelves_deep_num { get; set; }

        [SugarColumn(ColumnName = "max_load")]
        public int? max_load { get; set; }

        [SugarColumn(ColumnName = "upper_system_id", Length = 36)]
        public string? upper_system_id { get; set; }

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

        [SugarColumn(ColumnName = "point_height")]
        public int? point_height { get; set; }

        [SugarColumn(ColumnName = "point_width")]
        public int? point_width { get; set; }
    }
}
