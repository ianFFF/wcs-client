using System;

namespace wcs.core.model.Dto
{
    public class EquipmentAlarmTypeDto
    {
        public string id { get; set; }

        public string? alarm_type { get; set; }

        public string? alarm_code { get; set; }

        public string? alarm_level { get; set; }

        public string? alarm_remark { get; set; }
    }
}

