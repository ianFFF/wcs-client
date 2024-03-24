using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.DTOModel
{
    public class GiveRoleSetMenuDto
    {
        public List<long> RoleIds { get; set; }
        public List<long> MenuIds { get; set; }
    }
}
