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
    public partial class LoginLogService : BaseService<LoginLogEntity>, ILoginLogService
    {
        public async Task<PageModel<List<LoginLogEntity>>> SelctPageList(LoginLogEntity loginLog, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                    .WhereIF(!string.IsNullOrEmpty(loginLog.LoginIp), u => u.LoginIp.Contains(loginLog.LoginIp))
                        .WhereIF(!string.IsNullOrEmpty(loginLog.LoginUser), u => u.LoginUser.Contains(loginLog.LoginUser))
                     .WhereIF(loginLog.IsDeleted.IsNotNull(), u => u.IsDeleted == loginLog.IsDeleted)
                     .WhereIF(page.StartTime.IsNotNull() && page.EndTime.IsNotNull(), u => u.CreateTime >= page.StartTime && u.CreateTime <= page.EndTime)
                    .OrderBy(u => u.CreateTime, OrderByType.Desc)
                    .ToPageListAsync(page.PageNum, page.PageSize, total);
            return new PageModel<List<LoginLogEntity>>(data, total);
        }
    }
}
