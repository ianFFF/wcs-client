using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 大屏点位映射
    ///</summary>
    [SugarTable("wcs_point_map")]
    public partial class PointMapEntity : BaseV2ModelEntity
    {
        /// <summary>
        /// 点位类型 x、y 
        ///</summary>
        [SugarColumn(ColumnName = "point_type", Length = 10)]
        public string? point_type { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [SugarColumn(ColumnName = "value")]
        public int? value { get; set; }

        /// <summary>
        /// 最小真实值
        /// </summary>
        [SugarColumn(ColumnName = "min_realy")]
        public float? min_realy { get; set; }

        /// <summary>
        /// 最大真实值
        /// </summary>
        [SugarColumn(ColumnName = "max_realy")]
        public float? max_realy { get; set; }
    }
}
