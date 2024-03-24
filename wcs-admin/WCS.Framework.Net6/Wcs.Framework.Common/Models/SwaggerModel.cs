using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.Common.Models
{
    public class SwaggerModel
    {
        public SwaggerModel(string url, string name)
        {
            this.url = url;
            this.name = name;
        }
        public string url { get; set; }
        public string name { get; set; }
    }
}
