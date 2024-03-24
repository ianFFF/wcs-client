using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Model.Models;

namespace Wcs.Framework.DTOModel
{
    public class UserInfoDto
    {
        public UserEntity User { get; set; }
        public List<long> RoleIds { get; set; }
        public List<long> PostIds { get; set; }
        public long? DeptId { get; set; }
    }
}
