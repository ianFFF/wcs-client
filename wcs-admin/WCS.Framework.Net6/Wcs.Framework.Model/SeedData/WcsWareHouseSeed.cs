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
    public class WcsWareHouseSeed : AbstractSeed<WarehouseEntity>
    {
        public override List<WarehouseEntity> GetSeed()
        {
            WarehouseEntity type1 = new WarehouseEntity()
            {
                Id = Guid.NewGuid().ToString(),
                warehouse_name = "上海库区",
                IsDeleted = false
            };
            Entitys.Add(type1);

            return Entitys;
        }
    }
}
