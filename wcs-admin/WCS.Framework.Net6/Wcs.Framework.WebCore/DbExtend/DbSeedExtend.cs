﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Model.SeedData;

namespace Wcs.Framework.WebCore.DbExtend
{
    public static class DbSeedExtend
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="_Db"></param>
        /// <returns></returns>
        public static bool DataInvoer(ISqlSugarClient _Db)
        {
            bool res = false;
            var users = SeedFactory.GetUserSeed();
            var roles = SeedFactory.GetRoleSeed();
            var menus = SeedFactory.GetMenuSeed();
            var dicts = SeedFactory.GetDictionarySeed();
            var posts = SeedFactory.GetPostSeed();
            var dictinfos = SeedFactory.GetDictionaryInfoSeed();
            var depts = SeedFactory.GetDeptSeed();
            var files = SeedFactory.GetFileSeed();

            var equipmentType = SeedFactory.GetEquipmentTypeSeed();
            var taskType = SeedFactory.GetTaskTypeSeed();
            var wareHouse = SeedFactory.GetWareHouseSeed();
            var equipment = SeedFactory.GetEquipmentSeed();
            try
            {
                _Db.AsTenant().BeginTran();

                if (!_Db.Queryable<UserEntity>().Any())
                {
                    _Db.Insertable(users).ExecuteCommand();
                }

                if (!_Db.Queryable<RoleEntity>().Any())
                {
                    _Db.Insertable(roles).ExecuteCommand();
                }

                if (!_Db.Queryable<MenuEntity>().Any())
                {
                    _Db.Insertable(menus).ExecuteCommand();
                }

                if (!_Db.Queryable<DictionaryEntity>().Any())
                {
                    _Db.Insertable(dicts).ExecuteCommand();
                }
                if (!_Db.Queryable<PostEntity>().Any())
                {
                    _Db.Insertable(posts).ExecuteCommand();
                }
                if (!_Db.Queryable<DictionaryInfoEntity>().Any())
                {
                    _Db.Insertable(dictinfos).ExecuteCommand();
                }
                if (!_Db.Queryable<DeptEntity>().Any())
                {
                    _Db.Insertable(depts).ExecuteCommand();
                }

                if (!_Db.Queryable<UserRoleEntity>().Any())
                {
                    _Db.Insertable(SeedFactory.GetUserRoleSeed(users, roles)).ExecuteCommand();
                }

                if (!_Db.Queryable<RoleMenuEntity>().Any())
                {
                    _Db.Insertable(SeedFactory.GetRoleMenuSeed(roles, menus)).ExecuteCommand();
                }

                if (!_Db.Queryable<FileEntity>().Any())
                {
                    _Db.Insertable(files).ExecuteCommand();
                }

                if (!_Db.Queryable<EquipmentTypeEntity>().Any())
                {
                    _Db.Insertable(equipmentType).ExecuteCommand();
                }

                if (!_Db.Queryable<TaskTypeEntity>().Any())
                {
                    _Db.Insertable(taskType).ExecuteCommand();
                }

                if (!_Db.Queryable<WarehouseEntity>().Any())
                {
                    _Db.Insertable(wareHouse).ExecuteCommand();
                }

                if (!_Db.Queryable<EquipmentEntity>().Any())
                {
                    _Db.Insertable(equipment).ExecuteCommand();
                }

                _Db.AsTenant().CommitTran();
                res = true;
            }
            catch (Exception ex)
            {
                _Db.AsTenant().RollbackTran();//数据回滚
                Console.WriteLine(ex);

            }
            return res;
        }

        /// <summary>
        /// codeFirst初始化表
        /// </summary>
        /// <param name="_Db"></param>
        /// <returns></returns>
        public static void TableInvoer(ISqlSugarClient _Db)
        {
            //创建数据库
            _Db.DbMaintenance.CreateDatabase();
            var typeList = Common.Helper.AssemblyHelper.GetClass("Wcs.Framework.Model");
            foreach (var t in typeList)
            {
                //扫描如果存在SugarTable特性 并且 不是分表模型，直接codefirst
                if (t.GetCustomAttributes(false).Any(a => a.GetType().Equals(typeof(SugarTable))
                && !t.GetCustomAttributes(false).Any(a => a.GetType().Equals(typeof(SplitTableAttribute)))))
                {
                    _Db.CodeFirst.SetStringDefaultLength(200).InitTables(t);//这样一个表就能成功创建了
                }
            }

        }

        public static void UseDbSeedInitService(this IApplicationBuilder app)
        {

            if (Appsettings.appBool("DbCodeFirst_Enabled"))
            {
                var _Db = app.ApplicationServices.GetRequiredService<ISqlSugarClient>();
                TableInvoer(_Db);
            }

            if (Appsettings.appBool("DbSeed_Enabled"))
            {
                var _Db = app.ApplicationServices.GetRequiredService<ISqlSugarClient>();
                DataInvoer(_Db);
            }

        }
    }
}
