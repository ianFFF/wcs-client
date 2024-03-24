using System;

namespace Wcs.Framework.DTOModel
{
    public class TaskDto
    {
        public string Id { get; set; }

        public string task_type { get; set; }

        public DateTime? CreateTime { get; set; }

        public string begin_equipment_name { get; set; }

        public string process_equipment_name { get; set; }

        public string end_equipment_name { get; set; }

        public string begin_shelves_code { get; set; }

        public int? begin_shelves_position_row { get; set; }

        public int? begin_shelves_position_col { get; set; }

        public int? begin_shelves_position_point_x { get; set; }

        public int? begin_shelves_position_point_y { get; set; }

        public int? begin_shelves_position_point_z { get; set; }

        public string target_shelves_code { get; set; }

        public int? target_shelves_position_row { get; set; }

        public int? target_shelves_position_col { get; set; }

        public int? target_shelves_position_point_x { get; set; }

        public int? target_shelves_position_point_y { get; set; }

        public int? target_shelves_position_point_z { get; set; }

        public string task_status { get; set; }

        public string task_error { get; set; }

        public string task_code { get; set; }

        public string loader_code { get; set; }
    }
}

