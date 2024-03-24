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
    public partial class EquipmentService : BaseService<EquipmentEntity>, IEquipmentService
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentDto>>> SelctPageList(EquipmentEntity entity, PageParModel page)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .LeftJoin<EquipmentTypeEntity>((e, et) => e.equipment_type == et.Id)
                .LeftJoin<WarehouseEntity>((e, et, w) => e.warehouse_area == w.Id)
                .Where(e => e.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(entity.equipment_name), e => e.equipment_name.Contains(entity.equipment_name))
                .OrderBy(e => e.CreateTime, OrderByType.Desc)
                .Select((e, et, w) => new EquipmentDto
                {
                    Id = e.Id,
                    equipment_type = et.equipment_type_name,
                    equipment_name = e.equipment_name,
                    equipment_supplier = e.equipment_supplier,
                    upper_system_id = e.upper_system_id,
                    ip = e.ip,
                    port = e.port,
                    warehouse_area = w.warehouse_name
                })
                .ToPageListAsync(page.PageNum, page.PageSize, total);

            return new PageModel<List<EquipmentDto>>(data, total);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntity(EquipmentEntity entity)
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
        public async Task<EquipmentEntity> GetDetailById(string id)
        {
            return await _repository._DbQueryable.SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// 获取所有设备列表
        /// </summary>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentEntity>>> SelctList(string type)
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .Where(x => x.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(type), x => x.equipment_type == type)
                .OrderBy(x => x.CreateTime, OrderByType.Desc)
                .ToListAsync();

            return new PageModel<List<EquipmentEntity>>(data, total);
        }

        /// <summary>
        /// 获取所有X真实点信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<int?>> GetAllPointXReal()
        {
            return await _repository.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .GroupBy(x => new { x.point_x })
                .OrderBy(x => x.point_x, OrderByType.Asc)
                .Select(x => x.point_x)
                .ToListAsync();
        }

        /// <summary>
        /// 获取所有Y真实点信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<int?>> GetAllPointYReal()
        {
            // 该库Z为Y轴
            return await _repository.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .GroupBy(x => new { x.point_z })
                .OrderBy(x => x.point_z, OrderByType.Asc)
                .Select(x => x.point_z)
                .ToListAsync();
        }

        /// <summary>
        /// 手工操作-获取所有设备列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="relation_object_id"></param>
        /// <returns></returns>
        public async Task<List<EquipmentEntity>> SelctListForManual(string type, string relation_object_id)
        {
            return await _repository.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .WhereIF(type == "99ae0692-85b3-11ed-a5d9-00163e10b48b", x => x.io_point != "")
                .WhereIF(!string.IsNullOrEmpty(relation_object_id), x => x.relation_object_id == relation_object_id)
                .OrderBy(x => x.CreateTime, OrderByType.Asc)
                .ToListAsync();
        }
    }
}
