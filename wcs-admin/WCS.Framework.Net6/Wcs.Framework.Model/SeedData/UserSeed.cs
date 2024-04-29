using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Model.Models;

namespace Wcs.Framework.Model.SeedData
{
    public class UserSeed : AbstractSeed<UserEntity>
    {
        public override List<UserEntity> GetSeed()
        {
            UserEntity user1 = new UserEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                Name = "ian",
                UserName = "admin",
                Nick = "橙子",
                Password = "123456",
                Email = "",
                Phone = "",
                Sex = 0,
                Address = "深圳",
                Age = 20,
                Introduction = "还有谁？",
                OrderNum = 999,
                Remark = "描述是什么呢？",
                IsDeleted = false
            };
            user1.BuildPassword();
            Entitys.Add(user1);

            UserEntity user2 = new UserEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                Name = "fanwd",
                UserName = "fanwd",
                Nick = "测试",
                Password = "123456",
                Email = "",
                Phone = "",
                Sex = 0,
                Address = "深圳",
                Age = 18,
                Introduction = "还有我！",
                OrderNum = 1,
                Remark = "我没有描述！",
                IsDeleted = false
            };
            user2.BuildPassword();
            Entitys.Add(user2);

            UserEntity user3 = new UserEntity()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                Name = "wcs",
                UserName = "wcs",
                Nick = "wcs管理员",
                Password = "123456",
                Email = "",
                Phone = "",
                Sex = 0,
                Address = "深圳",
                Age = 18,
                Introduction = "",
                OrderNum = 1,
                Remark = "",
                IsDeleted = false
            };
            user3.BuildPassword();
            Entitys.Add(user3);

            return Entitys;
        }
    }
}
