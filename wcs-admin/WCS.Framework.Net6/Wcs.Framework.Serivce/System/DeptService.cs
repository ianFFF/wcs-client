using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class DeptService
    {
        public async Task<List<DeptEntity>> SelctGetList(DeptEntity dept)
        {
            var data = await _repository._DbQueryable
                    .WhereIF(!string.IsNullOrEmpty(dept.DeptName), u => u.DeptName.Contains(dept.DeptName))
                     .WhereIF(dept.IsDeleted.IsNotNull(), u => u.IsDeleted == dept.IsDeleted)
                    .OrderBy(u => u.OrderNum, OrderByType.Desc)
                   .ToListAsync();

            return data;
        }

        public async Task<List<DeptEntity>> GetListByRoleId(long roleId)
        {
            return (await _repository._Db.Queryable<RoleEntity>().Includes(r => r.Depts).SingleAsync(r => r.Id == roleId)).Depts;
        }
    }
}
