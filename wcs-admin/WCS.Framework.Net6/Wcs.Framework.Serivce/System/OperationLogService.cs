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
    public partial class OperationLogService : BaseService<OperationLogEntity>, IOperationLogService
    {
        public async Task<PageModel<List<OperationLogEntity>>> SelctPageList(OperationLogEntity operationLog, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                    .WhereIF(!string.IsNullOrEmpty(operationLog.Title), u => u.Title.Contains(operationLog.Title))
                        .WhereIF(!string.IsNullOrEmpty(operationLog.OperUser), u => u.OperUser.Contains(operationLog.OperUser))
                        .WhereIF(operationLog.OperType is not null, u => u.OperType==operationLog.OperType.GetHashCode())
                     .WhereIF(operationLog.IsDeleted.IsNotNull(), u => u.IsDeleted == operationLog.IsDeleted)
                     .WhereIF(page.StartTime.IsNotNull() && page.EndTime.IsNotNull(), u => u.CreateTime >= page.StartTime && u.CreateTime <= page.EndTime)
                    .OrderBy(u => u.CreateTime, OrderByType.Desc)
                    .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<OperationLogEntity>>(data, total);
        }
    }
}
