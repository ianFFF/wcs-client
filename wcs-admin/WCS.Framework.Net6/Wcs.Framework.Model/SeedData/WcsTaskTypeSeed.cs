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
    public class WcsTaskTypeSeed : AbstractSeed<TaskTypeEntity>
    {
        public override List<TaskTypeEntity> GetSeed()
        {
            TaskTypeEntity type1 = new TaskTypeEntity()
            {
                Id = "002cfc70-89a2-11ed-a5d9-00163e10b48b",
                task_type = "入库",
                IsDeleted = false
            };
            Entitys.Add(type1);

            TaskTypeEntity type2 = new TaskTypeEntity()
            {
                Id = "ba45546e-915b-11ed-a5d9-00163e10b48b",
                task_type = "出库",
                IsDeleted = false
            };
            Entitys.Add(type2);

            return Entitys;
        }
    }
}
