using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.Model.SeedData
{
    public abstract class AbstractSeed<T>
    {
        protected List<T> Entitys { get; set; } = new List<T>();
        public virtual List<T> GetSeed()
        {
            return Entitys;
        }
    }
}
