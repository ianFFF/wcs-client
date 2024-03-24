using SqlSugar;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.Common.Helper;

namespace Wcs.Framework.Service
{
    public partial class TaskService : BaseService<TaskEntity>, ITaskService
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<TaskDto>>> SelctPageList(TaskEntity entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<TaskTypeEntity>((task, taskType) => task.task_type == taskType.Id)

                .LeftJoin<ShelvesPositionEntity>((task, taskType, start_spe) => task.start_object == start_spe.Id)
                .LeftJoin<ShelvesEntity>((task, taskType, start_spe, start_se) => start_spe.shelves_id == start_se.Id)

                .LeftJoin<ShelvesPositionEntity>((task, taskType, start_spe, start_se, end_spe) => task.end_object == end_spe.Id)
                .LeftJoin<ShelvesEntity>((task, taskType, start_spe, start_se, end_spe, end_se) => end_spe.shelves_id == end_se.Id)

                .LeftJoin<EquipmentEntity>((task, taskType, start_spe, start_se, end_spe, end_se, start_ee) => task.start_object == start_ee.Id)
                .LeftJoin<EquipmentEntity>((task, taskType, start_spe, start_se, end_spe, end_se, start_ee, end_ee) => task.end_object == end_ee.Id)
                .LeftJoin<EquipmentEntity>((task, taskType, start_spe, start_se, end_spe, end_se, start_ee, end_ee, process_ee) => task.process_object == process_ee.Id)
                .Where(task => task.IsDeleted == false)
                .WhereIF(entity != null && !string.IsNullOrEmpty(entity.task_type), task => task.task_type == entity.task_type)
                .WhereIF(entity != null && !string.IsNullOrEmpty(entity.task_code), task => task.task_code.Contains(entity.task_code))
                .WhereIF(entity != null && !string.IsNullOrEmpty(entity.loader_code), task => task.loader_code.Contains(entity.loader_code))
                .OrderBy(task => task.CreateTime, OrderByType.Desc)
                .Select((task, taskType, start_spe, start_se, end_spe, end_se, start_ee, end_ee, process_ee) => new TaskDto
                {
                    Id = task.Id,
                    task_type = taskType.task_type,
                    CreateTime = task.CreateTime,
                    begin_equipment_name = start_ee.equipment_name,
                    process_equipment_name = process_ee.equipment_name,
                    end_equipment_name = end_ee.equipment_name,
                    begin_shelves_code = start_se.shelves_code,
                    begin_shelves_position_point_x = start_spe.point_x,
                    begin_shelves_position_point_y = start_spe.point_y,
                    begin_shelves_position_point_z = start_spe.point_z,
                    begin_shelves_position_col = start_spe.col,
                    begin_shelves_position_row = start_spe.row,

                    target_shelves_code = end_se.shelves_code,
                    target_shelves_position_point_x = end_spe.point_x,
                    target_shelves_position_point_y = end_spe.point_y,
                    target_shelves_position_point_z = end_spe.point_z,
                    target_shelves_position_col = end_spe.col,
                    target_shelves_position_row = end_spe.row,
                    task_status = task.task_status,
                    task_error = task.task_error,
                    task_code = task.task_code,
                    loader_code = task.loader_code
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<TaskDto>>(data, total);
        }

        /// <summary>
        /// 查询单个详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskEntity> GetDetailById(string id)
        {
            return await _repository._DbQueryable.SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteById(string id)
        {
            return await _repository.DeleteAsync(x => x.Id == id);
        }

        /// <summary>
        /// 获取任务统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<TaskStatisticsDto> TaskStatistics()
        {
            TaskStatisticsDto result = new TaskStatisticsDto();
            DateTime now = DateTime.Now;
            result.WeeklyStatistics = new StatisticsItem()
            {
                ckCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "WEEK"), DateHelper.GetTimeEndByType(now, "WEEK"), "ba45546e-915b-11ed-a5d9-00163e10b48b"),
                rkCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "WEEK"), DateHelper.GetTimeEndByType(now, "WEEK"), "002cfc70-89a2-11ed-a5d9-00163e10b48b")
            };

            result.MonthStatistics = new StatisticsItem()
            {
                ckCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "MONTH"), DateHelper.GetTimeEndByType(now, "MONTH"), "ba45546e-915b-11ed-a5d9-00163e10b48b"),
                rkCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "MONTH"), DateHelper.GetTimeEndByType(now, "MONTH"), "002cfc70-89a2-11ed-a5d9-00163e10b48b")
            };

            result.YearStatistics = new StatisticsItem()
            {
                ckCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "YEAR"), DateHelper.GetTimeEndByType(now, "YEAR"), "ba45546e-915b-11ed-a5d9-00163e10b48b"),
                rkCount = await this.taskStatistics(DateHelper.GetTimeStartByType(now, "YEAR"), DateHelper.GetTimeEndByType(now, "YEAR"), "002cfc70-89a2-11ed-a5d9-00163e10b48b")
            };

            return result;
        }

        /// <summary>
        /// 获取时间范围内指定任务类型的任务完成数量
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="task_type"></param>
        /// <returns></returns>
        private async Task<int> taskStatistics(DateTime? begin, DateTime? end, string task_type)
        {
            return await _repository.AsQueryable()
                 .Where(x => x.IsDeleted == false)
                 .Where(x => x.task_status == "结束")
                 .WhereIF(!string.IsNullOrEmpty(task_type), x => x.task_type == task_type)
                 .WhereIF(begin != null, x => x.CreateTime >= begin)
                 .WhereIF(end != null, x => x.CreateTime <= end)
                 .CountAsync();
        }

        /// <summary>
        /// 初始化任务信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitTask()
        {
            return await _repository.DeleteAsync(x => x.Id != "");
        }

        /// <summary>
        /// 获取最早的三条未结束任务
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskDto>> GetTop3Task()
        {
            return await _repository.AsQueryable()
                .LeftJoin<TaskTypeEntity>((task, taskType) => task.task_type == taskType.Id)
                .Where(task => task.IsDeleted == false)
                .Where(task => task.task_status != "结束")
                .OrderBy(task => task.CreateTime, OrderByType.Asc)
                .Take(3)
                .Select((task, taskType) => new TaskDto
                {
                    task_type = taskType.task_type,
                    task_status = task.task_status,
                    CreateTime = task.CreateTime
                })
                .ToListAsync();
        }
    }
}
