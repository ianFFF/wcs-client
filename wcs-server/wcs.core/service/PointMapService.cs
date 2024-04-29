using System;
using wcs.core.common;
using wcs.core.model;
using SqlSugar;
using wcs.core.model.Dto;

namespace wcs.core.service
{
    public class PointMapService
    {
        /// <summary>
        /// 获取所有映射点位
        /// </summary>
        /// <returns></returns>
        public List<PointMapEntity> GetAllPointMap()
        {
            return DbContext.db.Queryable<PointMapEntity>()

                .Where(x => x.IsDeleted == false)

                .ToList();
        }

        /// <summary>
        /// 获取映射点位
        /// </summary>
        /// <param name="type"></param>
        /// <param name="realyValue"></param>
        /// <returns></returns>
        public int GetPoint(string type, float realyValue, List<PointMapEntity> pointMaps)
        {
            PointMapEntity? item = pointMaps.Find(x => x.point_type == type && x.min_realy < realyValue && x.max_realy >= realyValue);

            return item == null ? 0 : item.value;
        }
    }
}

