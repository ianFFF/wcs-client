using System;
using wcs.core.common;
using wcs.core.model;
using SqlSugar;

namespace wcs.core.service
{
    public class EquipmentRecordsService
    {
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        public void AddRecords(EquipmentRecordsEntity model)
        {
            DbContext.db.Insertable<EquipmentRecordsEntity>(model).ExecuteCommand();
        }
    }
}
