using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 库区表
    ///</summary>
    [SugarTable("wcs_warehouse")]
    public partial class WarehouseEntity : BaseV2ModelEntity
    {
        /// <summary>
        /// 库区名称 
        ///</summary>
        [SugarColumn(ColumnName = "warehouse_name", Length = 50)]
        public string? warehouse_name { get; set; }
    }
}
