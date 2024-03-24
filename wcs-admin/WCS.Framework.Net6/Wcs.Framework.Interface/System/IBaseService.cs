using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Interface
{
    public interface IBaseService<T> where T: class,new()
    {
        public IRepository<T> _repository { get; set; }
    }
}
