using System;
using wcs.core.common;
using wcs.core.model;
using SqlSugar;

namespace wcs.core.service
{
    public class EquipmentService
    {
        /// <summary>
        /// 根据设备id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EquipmentEntity GetOneById(string id)
        {
            return DbContext.db.Queryable<EquipmentEntity>()
                .Where(x => x.IsDeleted == false && x.Id == id)
                .First();
        }

        /// <summary>
        /// 获取所有有效设备
        /// </summary>
        /// <returns></returns>
        public List<EquipmentEntity> GetAllEquipment()
        {
            return DbContext.db.Queryable<EquipmentEntity>()
                .Where(x => x.IsDeleted == false)
                .ToList();
        }
    }
}

