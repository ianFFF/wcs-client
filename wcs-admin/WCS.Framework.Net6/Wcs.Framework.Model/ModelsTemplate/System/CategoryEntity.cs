﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SqlSugar;
namespace Wcs.Framework.Model.Models
{
    /// <summary>
    /// 商品分类表
    ///</summary>
    [SugarTable("Category")]
    public partial class CategoryEntity:IBaseModelEntity
    {
        public CategoryEntity()
        {
            this.CreateTime = DateTime.Now;
        }
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true   )]
         public long Id { get; set; }
        /// <summary>
        /// 父级id 
        ///</summary>
         [SugarColumn(ColumnName="ParentId"    )]
         public long? ParentId { get; set; }
        /// <summary>
        /// 分类名称 
        ///</summary>
         [SugarColumn(ColumnName="CategoryName"    )]
         public string CategoryName { get; set; }
        /// <summary>
        /// 创建者 
        ///</summary>
         [SugarColumn(ColumnName="CreateUser"    )]
         public long? CreateUser { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
         [SugarColumn(ColumnName="CreateTime"    )]
         public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改者 
        ///</summary>
         [SugarColumn(ColumnName="ModifyUser"    )]
         public long? ModifyUser { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
         [SugarColumn(ColumnName="ModifyTime"    )]
         public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 是否删除 
        ///</summary>
         [SugarColumn(ColumnName="IsDeleted"    )]
         public bool? IsDeleted { get; set; }
        /// <summary>
        /// 租户Id 
        ///</summary>
         [SugarColumn(ColumnName="TenantId"    )]
         public long? TenantId { get; set; }
        /// <summary>
        /// 排序字段 
        ///</summary>
         [SugarColumn(ColumnName="OrderNum"    )]
         public int? OrderNum { get; set; }
        /// <summary>
        /// 描述 
        ///</summary>
         [SugarColumn(ColumnName="Remark"    )]
         public string? Remark { get; set; }
    }
}
