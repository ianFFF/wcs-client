using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 测试表
    ///</summary>
    [SugarTable("wcs_test")]
    public partial class WcstestEntity : BaseV2ModelEntity
    {
    }
}
