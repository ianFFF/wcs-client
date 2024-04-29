using System;
using SqlSugar;

namespace wcs.core.model
{
    [SugarTable("wcs_shelves_position")]
    public partial class ShelvesPositionEntity : BaseModel
    {
        [SugarColumn(ColumnName = "shelves_id", Length = 36)]
        public string? shelves_id { get; set; }

        [SugarColumn(ColumnName = "point_x")]
        public int? point_x { get; set; }

        [SugarColumn(ColumnName = "point_y")]
        public int? point_y { get; set; }

        [SugarColumn(ColumnName = "point_z")]
        public int? point_z { get; set; }

        [SugarColumn(ColumnName = "is_loaded")]
        public bool? is_loaded { get; set; }

        [SugarColumn(ColumnName = "col")]
        public int? col { get; set; }

        [SugarColumn(ColumnName = "row")]
        public int? row { get; set; }

        [SugarColumn(ColumnName = "is_locked")]
        public bool? is_locked { get; set; }

        [SugarColumn(ColumnName = "status")]
        public int? status { get; set; }
    }
}
