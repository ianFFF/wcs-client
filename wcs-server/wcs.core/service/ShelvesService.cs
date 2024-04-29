using System;
using wcs.core.common;
using wcs.core.model;
using SqlSugar;
using wcs.core.model.Dto;

namespace wcs.core.service
{
    public class ShelvesService
    {
        /// <summary>
        /// 获取所有货位点位
        /// </summary>
        /// <returns></returns>
        public List<ShelvesPositionDto> GetAllSheivesPosition()
        {
            return DbContext.db.Queryable<ShelvesPositionEntity>()
                .LeftJoin<ShelvesEntity>((spe, se) => spe.shelves_id == se.Id)
                .Where(spe => spe.IsDeleted == false)
                .Select((spe, se) => new ShelvesPositionDto
                {
                    id = spe.Id,
                    shelves_code = se.shelves_code,
                    relation_object_id = se.relation_object_id,
                    shelves_id = spe.shelves_id,
                    point_x = spe.point_x,
                    point_y = spe.point_y,
                    point_z = spe.point_z,
                    is_loaded = spe.is_loaded,
                    is_locked = spe.is_locked,
                    row = spe.row,
                    col = spe.col,
                    status = spe.status
                })
                .ToList();
        }

        /// <summary>
        /// 更新货位装载情况
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLoaded"></param>
        public void UpdateSheviesPostionLoad(string id, bool isLoaded)
        {
            DbContext.db.Updateable<ShelvesPositionEntity>()
                .SetColumns(x => x.is_loaded == isLoaded)
                .Where(x => x.Id == id)
                .ExecuteCommand();
        }

        /// <summary>
        /// 更新货位锁定情况
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLocked"></param>
        public void UpdateSheviesPositionLock(string id, bool isLocked)
        {
            DbContext.db.Updateable<ShelvesPositionEntity>()
                .SetColumns(x => x.is_locked == isLocked)
                .Where(x => x.Id == id)
                .ExecuteCommand();
        }
    }
}

