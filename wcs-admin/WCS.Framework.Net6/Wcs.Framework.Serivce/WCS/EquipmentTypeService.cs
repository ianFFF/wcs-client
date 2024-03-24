using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentTypeService : BaseService<EquipmentTypeEntity>, IEquipmentTypeService
    {
        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageModel<List<EquipmentTypeEntity>>> SelctList()
        {
            RefAsync<int> total = 0;
            var data = await _repository._DbQueryable
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CreateTime, OrderByType.Desc)
                .ToListAsync();

            return new PageModel<List<EquipmentTypeEntity>>(data, total);
        }

        /// <summary>
        /// 根据设备类型获取所有设备异常类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<AlarmTypeVo>> GetAlarmType()
        {
            List<AlarmTypeVo> data = await _repository._DbQueryable
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CreateTime, OrderByType.Desc)
                .Select((x) => new AlarmTypeVo
                {
                    alarm_type = x.equipment_type_name + "类异常"
                }).ToListAsync();

            data.Add(new AlarmTypeVo
            {
                alarm_type = "系统类异常"
            });

            return data;
        }
    }
}
