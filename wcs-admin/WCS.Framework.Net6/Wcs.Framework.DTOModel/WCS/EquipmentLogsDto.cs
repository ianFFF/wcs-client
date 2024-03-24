using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Wcs.Framework.DTOModel
{
    public class EquipmentLogsDto
    {
        public string Id { get; set; }

        public string equipment_name { get; set; }

        public int? logs_interval { get; set; }

        public int? logs_interval_type { get; set; }

        public bool? logs_is_record { get; set; }
    }
}
