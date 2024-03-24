using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class ShelvesPositionService : BaseService<ShelvesPositionEntity>, IShelvesPositionService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntity(ShelvesPositionEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
            entity.is_loaded = false;
            entity.is_locked = false;
            entity.status = 0;

            return await _repository.InsertAsync(entity);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEntity(ShelvesPositionEntity entity)
        {
            return await _repository.AsUpdateable()
                .Where(x => x.Id == entity.Id)
                .SetColumnsIF(entity.is_locked != null, x => x.is_locked == entity.is_locked)
                .SetColumnsIF(entity.is_loaded != null, x => x.is_loaded == entity.is_loaded)
                .SetColumnsIF(entity.status != null, x => x.status == entity.status)
                .ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 根据货架刷新点位
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEntityByShelves(ShelvesEntity entity)
        {
            await _repository.DeleteAsync(x => x.shelves_id == entity.Id);

            return await this.AddEntityByShelves(entity);
        }

        /// <summary>
        /// 根据货架获取所有货位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataTable> GetAllPositionByShelvesId(string id)
        {
            DataTable data = await _repository._Db.Ado.GetDataTableAsync(@"
                select *
                from wcs_shelves_position as a
                         left join (select end_object,
                                           task_code,
                                           loader_code,
                                           CompleteTime
                                    from (select end_object,
                                                 task_code,
                                                 loader_code,
                                                 CompleteTime,
                                                 row_number() over (partition by end_object order by CreateTime desc) as rowNum
                                          from wcs_task
                                          where task_type = '002cfc70-89a2-11ed-a5d9-00163e10b48b') as t
                                    where rowNum = 1) as b on b.end_object = a.Id
                where IsDeleted = false and shelves_id = @shelvesId
                order by a.row desc, a.col desc
            ", new { shelvesId = id });

            return data;
            //return await _repository.AsQueryable()
            //    .Where(x => x.IsDeleted == false)
            //    .Where(x => x.shelves_id == id)
            //    .OrderBy(x => x.row, OrderByType.Desc)
            //    .OrderBy(x => x.col, OrderByType.Desc)
            //    .ToListAsync();
        }

        /// <summary>
        /// 根据货架生成点位
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddEntityByShelves(ShelvesEntity entity)
        {
            int col = (int)(entity.shelves_cols_num == null ? 0 : entity.shelves_cols_num);
            int row = (int)(entity.shelves_rows_num == null ? 0 : entity.shelves_rows_num);

            int curX = (int)(entity.point_x == null ? 0 : entity.point_x);
            int curY = 0;
            int height = (int)(entity.point_height == null ? 0 : entity.point_height);

            for (int _col = 0; _col < col; _col++)
            {
                curY = 0;
                for (int _row = 0; _row < row; _row++)
                {
                    ShelvesPositionEntity point = new ShelvesPositionEntity();
                    point.shelves_id = entity.Id;
                    point.point_z = entity.point_z;
                    point.point_x = curX;
                    point.col = _col + 1;
                    point.point_y = curY;
                    point.row = _row + 1;
                    await this.AddEntity(point);

                    curY += height;
                }

                // 计算下一个x坐标
                if (_col % 2 == 0)
                {
                    curX += 1320;
                }
                else
                {
                    curX += 1460;
                }
            }

            return true;
        }

        /// <summary>
        /// 删除点位
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEntityByShelves(ShelvesEntity entity)
        {
            return await _repository.DeleteAsync(x => x.shelves_id == entity.Id);
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
        /// 初始化仓库货位状态
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitLockedLoaded()
        {
            return await _repository.AsUpdateable()
               .Where(x => x.IsDeleted == false)
               .SetColumns(x => x.is_locked == false)
               .SetColumns(x => x.is_loaded == false)
               .ExecuteCommandAsync() > 0;
        }
    }
}
