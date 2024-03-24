using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.Model.Models
{
    public interface IBaseV2ModelEntity
    {
        public string Id { get; set; }

        public string? CreateUser { get; set; }

        public string? ModifyUser { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
