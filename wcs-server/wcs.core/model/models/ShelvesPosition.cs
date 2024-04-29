using System;

namespace wcs.core.model
{
    public class ShelvesPosition
    {
        public string id { get; set; }

        public string relation_object_id { get; set; }

        public string shelves_id { get; set; }

        public string shelves_name { get; set; }

        public int point_x { get; set; }

        public int point_y { get; set; }

        public int point_z { get; set; }

        public bool is_loaded { get; set; }

        public bool is_locked { get; set; }

        public string layout_id { get; set; }

        public string layout_x { get; set; }

        public string layout_y { get; set; }

        public int status { get; set; }
    }
}

