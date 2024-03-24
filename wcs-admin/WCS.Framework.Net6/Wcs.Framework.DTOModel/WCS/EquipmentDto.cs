using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Wcs.Framework.DTOModel
{
    public class EquipmentDto
    {
        public string Id { get; set; }

        public string equipment_type { get; set; }

        public string equipment_name { get; set; }

        public string equipment_supplier { get; set; }

        public string upper_system_id { get; set; }

        public string ip { get; set; }

        public string port { get; set; }

        public string warehouse_area { get; set; }
    }
}
