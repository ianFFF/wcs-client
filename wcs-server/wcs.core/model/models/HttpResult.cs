using System;
namespace wcs.core.model
{
    public class HttpResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public object data { get; set; }
    }
}

