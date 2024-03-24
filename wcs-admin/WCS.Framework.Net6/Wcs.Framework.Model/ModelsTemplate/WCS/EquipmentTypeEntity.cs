using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 设备类型对象
    ///</summary>
    [SugarTable("wcs_equipment_type")]
    public partial class EquipmentTypeEntity : BaseV2ModelEntity
    {
        /// <summary>
        /// 类型名称 
        ///</summary>
        [SugarColumn(ColumnName = "equipment_type_name", Length = 50)]
        public string? equipment_type_name { get; set; }
    }
}
