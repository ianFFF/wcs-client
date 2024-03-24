using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Wcs.Framework.DTOModel
{
    public class EquipmentRecordsDto
    {
        public string Id { get; set; }

        public string? alarm_level { get; set; }

        public string? alarm_type { get; set; }

        public string? alarm_code { get; set; }

        public string? message { get; set; }

        public string equipment_name { get; set; }

        public string task_code { get; set; }

        public string loader_code { get; set; }

        public string task_status { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? TaskCreateTime { get; set; }

        public DateTime? TaskCompleteTime { get; set; }

        public DateTime? SearchBeginTime { get; set; }

        public DateTime? SearchEndTime { get; set; }
    }
}
