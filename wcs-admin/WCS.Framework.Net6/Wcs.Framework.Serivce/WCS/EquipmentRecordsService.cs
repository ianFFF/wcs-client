using SqlSugar;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentRecordsService : BaseService<EquipmentRecordsEntity>, IEquipmentRecordsService
    {
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentRecordsDto>>> SelctPageList(EquipmentRecordsDto entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<AlarmLevelEntity>((wer, wal) => wer.alarm_level == wal.Id)
                .LeftJoin<TaskEntity>((wer, wal, wte) => wer.task_id == wte.Id)
                .Where(wer => wer.IsDeleted == false)
                .WhereIF(string.IsNullOrEmpty(entity.alarm_code), wer => wer.alarm_code == null)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_code) && entity.alarm_code == "error", wer => wer.alarm_code != null)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_code) && entity.alarm_code != "error", wer => wer.alarm_code == entity.alarm_code)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_type), wer => wer.alarm_type == entity.alarm_type)
                .WhereIF(entity.SearchBeginTime != null, wer => wer.CreateTime >= entity.SearchBeginTime)
                .WhereIF(entity.SearchEndTime != null, wer => wer.CreateTime <= entity.SearchEndTime)
                .OrderBy(wer => wer.CreateTime, OrderByType.Desc)
                .Select((wer, wal, wte) => new EquipmentRecordsDto
                {
                    Id = wer.Id,
                    alarm_level = wal.alarm_level,
                    alarm_code = wer.alarm_code,
                    alarm_type = wer.alarm_type,
                    message = wer.message,
                    CreateTime = wer.CreateTime,
                    task_code = wte.task_code,
                    task_status = wte.task_status,
                    TaskCreateTime = wte.CreateTime,
                    TaskCompleteTime = wte.CompleteTime,
                    loader_code = wte.loader_code
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<EquipmentRecordsDto>>(data, total);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<EquipmentRecordsExcelDto>> ExportList(EquipmentRecordsDto entity)
        {
            var data = await _repository._DbQueryable
                .LeftJoin<AlarmLevelEntity>((wer, wal) => wer.alarm_level == wal.Id)
                .LeftJoin<TaskEntity>((wer, wal, wte) => wer.task_id == wte.Id)
                .Where(wer => wer.IsDeleted == false)
                .WhereIF(string.IsNullOrEmpty(entity.alarm_code), wer => wer.alarm_code == null)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_code) && entity.alarm_code == "error", wer => wer.alarm_code != null)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_code) && entity.alarm_code != "error", wer => wer.alarm_code == entity.alarm_code)
                .WhereIF(!string.IsNullOrEmpty(entity.alarm_type), wer => wer.alarm_type == entity.alarm_type)
                .WhereIF(entity.SearchBeginTime != null, wer => wer.CreateTime >= entity.SearchBeginTime)
                .WhereIF(entity.SearchEndTime != null, wer => wer.CreateTime <= entity.SearchEndTime)
                .OrderBy(wer => wer.CreateTime, OrderByType.Desc)
                .Select((wer, wal, wte) => new EquipmentRecordsExcelDto
                {
                    Id = wer.Id,
                    异常等级 = wal.alarm_level,
                    异常编码 = wer.alarm_code,
                    异常类型 = wer.alarm_type,
                    日志信息 = wer.message,
                    日志时间 = wer.CreateTime.ToString(),
                    任务流水号 = wte.task_code,
                    执行结果 = wte.task_status,
                    任务发起时间 = wte.CreateTime.ToString(),
                    任务完成时间 = wte.CompleteTime.ToString(),
                    托盘码 = wte.loader_code
                })
                .ToListAsync();

            return data;
        }
    }
}
