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
    public partial class EquipmentLogsService : BaseService<EquipmentLogsEntity>, IEquipmentLogsService
    {
        /// <summary>
        /// 获取设备日志配置列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentLogsDto>>> SelctPageList(EquipmentLogsDto entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<EquipmentEntity>((el, e) => el.equipment_id == e.Id)
                .Where(el => el.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(entity.equipment_name), (el, e) => e.equipment_name.Contains(entity.equipment_name))
                .OrderBy(el => el.CreateTime, OrderByType.Desc)
                .Select((el, e) => new EquipmentLogsDto
                {
                    Id = el.Id,
                    equipment_name = e.equipment_name,
                    logs_interval = el.logs_interval,
                    logs_interval_type = el.logs_interval_type,
                    logs_is_record = el.logs_is_record
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<EquipmentLogsDto>>(data, total);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntity(EquipmentLogsEntity entity)
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
        public async Task<EquipmentLogsEntity> GetDetailById(string id)
        {
            return await _repository._DbQueryable.SingleAsync(x => x.Id == id);
        }
    }
}
