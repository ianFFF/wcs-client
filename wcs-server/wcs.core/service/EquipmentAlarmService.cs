using System;
using wcs.core.model;
using wcs.core.model.Dto;
using SqlSugar;
using wcs.core.common;

namespace wcs.core.service
{
    public class EquipmentAlarmService
    {
        public EquipmentAlarmTypeDto getAlarmByCode(string code)
        {
            return DbContext.db.Queryable<EquipmentAlarmEntity>()
                .LeftJoin<EquipmentEntity>((ea, e) => ea.equipment_id == e.Id)
                .LeftJoin<EquipmentTypeEntity>((ea, e, et) => e.equipment_type == et.Id)
                .Where(ea => ea.IsDeleted == false && ea.alarm_code == code)
                .Select((ea, e, et) => new EquipmentAlarmTypeDto
                {
                    id = ea.Id,
                    alarm_code = ea.alarm_code,
                    alarm_level = ea.alarm_level,
                    alarm_remark = ea.alarm_remark,
                    alarm_type = $"{et.equipment_type_name}类异常"
                })
                .First();
        }
    }
}

