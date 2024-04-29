using System;
using SqlSugar;

namespace wcs.core.model
{
    /// <summary>
    /// 设备类型对象
    ///</summary>
    [SugarTable("wcs_equipment_type")]
    public partial class EquipmentTypeEntity : BaseModel
    {
        /// <summary>
        /// 类型名称 
        ///</summary>
        [SugarColumn(ColumnName = "equipment_type_name", Length = 50)]
        public string? equipment_type_name { get; set; }
    }
}

