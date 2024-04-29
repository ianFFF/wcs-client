using System;

namespace wcs.core.model.Dto
{
    public class ShelvesPositionDto
    {
        public string id { get; set; }

        public string? shelves_code { get; set; }

        public string? relation_object_id { get; set; }

        public string? shelves_id { get; set; }

        public int? point_x { get; set; }

        public int? point_y { get; set; }

        public int? point_z { get; set; }

        public bool? is_loaded { get; set; }

        public bool? is_locked { get; set; }

        public int? col { get; set; }

        public int? row { get; set; }

        public int? status { get; set; }
    }
}

