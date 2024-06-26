﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Common.Enum;
using Wcs.Framework.Model.Models;

namespace Wcs.Framework.Model.SeedData
{
    public class RoleSeed : AbstractSeed<RoleEntity>
    {
        public override List<RoleEntity> GetSeed()
        {
            RoleEntity role1 = new RoleEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                RoleName = "管理员",
                RoleCode = "admin",
                DataScope = DataScopeEnum.ALL.GetHashCode(),
                OrderNum = 999,
                Remark = "管理员",
                IsDeleted = false
            };
            Entitys.Add(role1);

            RoleEntity role2 = new RoleEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                RoleName = "测试角色",
                RoleCode = "test",
                DataScope = DataScopeEnum.ALL.GetHashCode(),
                OrderNum = 1,
                Remark = "测试用的角色",
                IsDeleted = false
            };
            Entitys.Add(role2);

            RoleEntity role3 = new RoleEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                RoleName = "wcs管理员",
                RoleCode = "wcsadmin",
                DataScope = DataScopeEnum.ALL.GetHashCode(),
                OrderNum = 2,
                Remark = "wcs管理员",
                IsDeleted = false
            };
            Entitys.Add(role3);

            return Entitys;
        }
    }
}
