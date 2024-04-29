using System;
namespace wcs.core.model
{
    public class Point
    {
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }

        public string layout_x { get; set; }
        public string layout_y { get; set; }

        public Point() { }

        public Point(string _x, string _y, string _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }
}

