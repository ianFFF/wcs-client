using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Wcs.Framework.Model.Models;
using System.Data;

namespace Wcs.Framework.DTOModel
{
    public class ShelvesDto
    {
        public string Id { get; set; }

        public string shelves_code { get; set; }

        public int? shelves_cols_num { get; set; }

        public int? shelves_rows_num { get; set; }

        public int? shelves_deep_num { get; set; }

        public int? max_load { get; set; }

        public string upper_system_id { get; set; }

        public string warehouse_area { get; set; }

        public DataTable shelvesPosition { get; set; }
    }
}
