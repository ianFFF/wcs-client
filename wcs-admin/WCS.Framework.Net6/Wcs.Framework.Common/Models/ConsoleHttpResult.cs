using System;
namespace Wcs.Framework.Common.Models
{
    public class ConsoleHttpResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public object data { get; set; }
    }
}
