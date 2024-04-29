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
    public class WcsEquipmentTypeSeed : AbstractSeed<EquipmentTypeEntity>
    {
        public override List<EquipmentTypeEntity> GetSeed()
        {
            //EquipmentTypeEntity type1 = new EquipmentTypeEntity()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    equipment_type_name = "PLC",
            //    IsDeleted = false
            //};
            //Entitys.Add(type1);

            EquipmentTypeEntity type2 = new EquipmentTypeEntity()
            {
                Id = "99ae0692-85b3-11ed-a5d9-00163e10b48b",
                equipment_type_name = "输送线",
                IsDeleted = false
            };
            Entitys.Add(type2);

            EquipmentTypeEntity type3 = new EquipmentTypeEntity()
            {
                Id = "cfee1e01-7ddd-11ed-a5d9-00163e10b48b",
                equipment_type_name = "堆垛机",
                IsDeleted = false
            };
            Entitys.Add(type3);

            return Entitys;
        }
    }
}
