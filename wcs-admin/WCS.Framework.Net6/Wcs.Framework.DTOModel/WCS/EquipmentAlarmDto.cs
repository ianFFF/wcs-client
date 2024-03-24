using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Wcs.Framework.DTOModel
{
    public class EquipmentAlarmDto
    {
        public string Id { get; set; }

        public string equipment_type { get; set; }

        public string equipment_name { get; set; }

        public string? alarm_level { get; set; }

        public string? alarm_code { get; set; }

        public string? alarm_remark { get; set; }
    }
}
