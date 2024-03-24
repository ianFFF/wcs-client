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
    public partial class ShelvesService : BaseService<ShelvesEntity>, IShelvesService
    {
        /// <summary>
        /// 获取货架列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<ShelvesDto>>> SelctPageList(ShelvesEntity entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<WarehouseEntity>((s, w) => s.warehouse_area == w.Id)
                .Where(s => s.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(entity.shelves_code), s => s.shelves_code.Contains(entity.shelves_code))
                .OrderBy(s => s.CreateTime, OrderByType.Desc)
                .Select((s, w) => new ShelvesDto
                {
                    Id = s.Id,
                    shelves_code = s.shelves_code,
                    shelves_cols_num = s.shelves_cols_num,
                    shelves_rows_num = s.shelves_rows_num,
                    shelves_deep_num = s.shelves_deep_num,
                    max_load = s.max_load,
                    upper_system_id = s.upper_system_id,
                    warehouse_area = w.warehouse_name
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<ShelvesDto>>(data, total);
        }

        /// <summary>
        /// 手工操作-根据设备获取关联同样设备的货架
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<List<ShelvesEntity>> SelctListForManual(string equipmentId)
        {
            return await _repository.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(equipmentId), x => x.relation_object_id == equipmentId)
                .OrderBy(x => x.CreateTime, OrderByType.Asc)
                .ToListAsync();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntity(ShelvesEntity entity)
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
        public async Task<ShelvesEntity> GetDetailById(string id)
        {
            return await _repository._DbQueryable.SingleAsync(x => x.Id == id);
        }
    }
}
