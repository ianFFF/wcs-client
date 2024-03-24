using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentAlarmService : BaseService<EquipmentAlarmEntity>, IEquipmentAlarmService
    {
        /// <summary>
        /// 获取设备日志配置列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentAlarmDto>>> SelctPageList(EquipmentAlarmDto entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<EquipmentEntity>((ea, e) => ea.equipment_id == e.Id)
                .LeftJoin<AlarmLevelEntity>((ea, e, at) => ea.alarm_level == at.Id)
                .LeftJoin<EquipmentTypeEntity>((ea, e, at, et) => ea.equipment_type_id == et.Id)
                .Where(ea => ea.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(entity.equipment_name), (ea, e, at, et) => e.equipment_name.Contains(entity.equipment_name))
                .OrderBy(ea => ea.CreateTime, OrderByType.Desc)
                .Select((ea, e, at, et) => new EquipmentAlarmDto
                {
                    Id = ea.Id,
                    equipment_type = et.equipment_type_name,
                    equipment_name = e.equipment_name,
                    alarm_level = at.alarm_level,
                    alarm_code = ea.alarm_code,
                    alarm_remark = ea.alarm_remark
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<EquipmentAlarmDto>>(data, total);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntity(EquipmentAlarmEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;

            return await _repository.InsertAsync(entity);
        }

        /// <summary>
        /// 查询单个详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquipmentAlarmEntity> GetDetailById(string id)
        {
            return await _repository._DbQueryable.SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// 获取所有异常编码
        /// </summary>
        /// <returns></returns>
        public async Task<List<EquipmentAlarmDto>> GetAlarmCode()
        {
            var data = await _repository._DbQueryable
                .Where(ea => ea.IsDeleted == false)
                .OrderBy(ea => ea.CreateTime, OrderByType.Desc)
                .Select((ea) => new EquipmentAlarmDto
                {
                    alarm_code = ea.alarm_code
                })
                .ToListAsync();

            data.Add(new EquipmentAlarmDto { alarm_code = "SYSERROR" });

            return data;
        }
    }
}
