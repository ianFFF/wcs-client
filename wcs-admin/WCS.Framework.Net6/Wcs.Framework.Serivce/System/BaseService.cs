using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Common.Attribute;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    [AppService]
    public class BaseService<T>:IBaseService<T> where T:class,new()
    {
        public IRepository<T> _repository { get; set; }
        public BaseService(IRepository<T> iRepository)
        {
            _repository = iRepository;
        }
    }
}
