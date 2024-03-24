﻿using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class EquipmentAlarmService : BaseService<EquipmentAlarmEntity>, IEquipmentAlarmService
    {
        public EquipmentAlarmService(IRepository<EquipmentAlarmEntity> repository) : base(repository)
        {
        }
    }
}
