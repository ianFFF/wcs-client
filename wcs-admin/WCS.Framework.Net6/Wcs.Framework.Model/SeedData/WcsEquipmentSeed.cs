using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Common.Enum;
using Wcs.Framework.Model.Models;

namespace Wcs.Framework.Model.SeedData
{
    public class WcsEquipmentSeed : AbstractSeed<EquipmentEntity>
    {
        public override List<EquipmentEntity> GetSeed()
        {
            EquipmentEntity equipment = new EquipmentEntity()
            {
                Id = "aac89211-a3b1-4699-9b13-86485d0de7df",
                equipment_type = "cfee1e01-7ddd-11ed-a5d9-00163e10b48b",
                equipment_name = "堆垛机（演示用勿删除）",
                point_x = 2000,
                point_y = 0,
                point_z = 2000,
                ip = "127.0.0.1",
                IsDeleted = false
            };
            Entitys.Add(equipment);

            EquipmentEntity ssx1 = new EquipmentEntity()
            {
                Id = Guid.NewGuid().ToString(),
                equipment_type = "99ae0692-85b3-11ed-a5d9-00163e10b48b",
                equipment_name = "输送线A（演示用勿删除）",
                point_x = 1000,
                point_y = 0,
                point_z = 1000,
                relation_object_id = equipment.Id,
                ip = "127.0.0.1",
                IsDeleted = false
            };
            Entitys.Add(ssx1);

            EquipmentEntity ssx2 = new EquipmentEntity()
            {
                Id = Guid.NewGuid().ToString(),
                equipment_type = "99ae0692-85b3-11ed-a5d9-00163e10b48b",
                equipment_name = "输送线B（演示用勿删除）",
                point_x = 2000,
                point_y = 0,
                point_z = 1000,
                io_point = "DB1",
                relation_object_id = equipment.Id,
                ip = "127.0.0.1",
                IsDeleted = false
            };
            Entitys.Add(ssx2);

            EquipmentEntity ssx3 = new EquipmentEntity()
            {
                Id = Guid.NewGuid().ToString(),
                equipment_type = "99ae0692-85b3-11ed-a5d9-00163e10b48b",
                equipment_name = "输送线C（演示用勿删除）",
                point_x = 1000,
                point_y = 0,
                point_z = 3000,
                relation_object_id = equipment.Id,
                ip = "127.0.0.1",
                IsDeleted = false
            };
            Entitys.Add(ssx3);

            EquipmentEntity ssx4 = new EquipmentEntity()
            {
                Id = Guid.NewGuid().ToString(),
                equipment_type = "99ae0692-85b3-11ed-a5d9-00163e10b48b",
                equipment_name = "输送线D（演示用勿删除）",
                point_x = 2000,
                point_y = 0,
                point_z = 3000,
                relation_object_id = equipment.Id,
                ip = "127.0.0.1",
                IsDeleted = false
            };
            Entitys.Add(ssx4);


            return Entitys;
        }
    }
}
