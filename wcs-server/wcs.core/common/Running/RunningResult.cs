using System;
namespace wcs.core.common.Running
{
    public class RunningResult
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string error_code { get; set; }

        public RunningResult()
        {
            this.result = true;
            this.message = "";
            this.error_code = "";
        }
    }
}

