using System;
using SqlSugar;
using System.Text.Json.Serialization;

namespace Wcs.Framework.Model.Models
{
    public class BaseV2ModelEntity : IBaseV2ModelEntity
    {
        public BaseV2ModelEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
        }

        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, Length = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 创建者 
        ///</summary>
        [SugarColumn(ColumnName = "CreateUser", Length = 36)]
        public string? CreateUser { get; set; }

        /// <summary>
        /// 创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改者 
        ///</summary>
        [SugarColumn(ColumnName = "ModifyUser", Length = 36)]
        public string? ModifyUser { get; set; }

        /// <summary>
        /// 修改时间 
        ///</summary>
        [SugarColumn(ColumnName = "ModifyTime")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 是否删除 
        ///</summary>
        [SugarColumn(ColumnName = "IsDeleted")]
        public bool? IsDeleted { get; set; }
    }
}

