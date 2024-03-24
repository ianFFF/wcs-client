using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.Service;
using Wcs.Framework.WebCore;
using Wcs.Framework.WebCore.AttributeExtend;
using Wcs.Framework.WebCore.AuthorizationPolicy;

namespace Wcs.Framework.ApiMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShelvesController : BaseSimpleCrudController<ShelvesEntity>
    {
        private readonly IShelvesService _iShelvesService;
        private readonly IEquipmentTypeService _iEquipmentTypeService;
        private readonly IEquipmentService _iEquipmentService;
        private readonly IWarehouseService _iWarehouseService;
        private readonly IShelvesPositionService _iShelvesPositionService;
        private readonly IPointMapService _iPointMapService;
        private readonly IMapper _mapper;

        public ShelvesController(
            ILogger<ShelvesEntity> logger,
            IShelvesService iShelvesService,
            IEquipmentTypeService iEquipmentTypeService,
            IEquipmentService iEquipmentService,
            IWarehouseService iWarehouseService,
            IShelvesPositionService iShelvesPositionService,
            IPointMapService iPointMapService,
            IMapper mapper
        ) : base(logger, iShelvesService)
        {
            this._iShelvesService = iShelvesService;
            this._iEquipmentTypeService = iEquipmentTypeService;
            this._iWarehouseService = iWarehouseService;
            this._iShelvesPositionService = iShelvesPositionService;
            this._iEquipmentService = iEquipmentService;
            this._iPointMapService = iPointMapService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取货架列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] ShelvesEntity entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iShelvesService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 手工操作-根据设备获取关联同样设备的货架
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> SelctListForManual([FromQuery] string? equipmentId)
        {
            List<ShelvesEntity> shelves = new List<ShelvesEntity>();
            string? relation_object_id = "";
            if (!string.IsNullOrEmpty(equipmentId))
            {
                EquipmentEntity equipment = await _iEquipmentService.GetDetailById(equipmentId);
                relation_object_id = equipment.relation_object_id;
            }

            shelves = await _iShelvesService.SelctListForManual(relation_object_id);

            return Result.Success().SetData(
                new
                {
                    shelves = _mapper.Map<List<ShelvesVo>>(shelves)
                }
            );
        }

        /// <summary>
        /// 根据货架获取所有点位信息
        /// </summary>
        /// <param name="shelvesId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> ShelevsPositionList([FromQuery] string shelvesId)
        {
            // 获取当前货架信息
            ShelvesEntity item = await _iShelvesService.GetDetailById(shelvesId);

            ShelvesDto shelves = new ShelvesDto()
            {
                Id = item.Id,
                shelves_code = item.shelves_code,
                shelves_rows_num = item.shelves_rows_num,
                shelves_cols_num = item.shelves_cols_num,

                // 获取当前货架下货位信息
                shelvesPosition = await _iShelvesPositionService.GetAllPositionByShelvesId(shelvesId)
            };

            return Result.Success().SetData(shelves);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<Result> Add(ShelvesEntity entity)
        {
            bool result = await _iShelvesService.AddEntity(entity);

            if (result)
            {
                await _iShelvesPositionService.AddEntityByShelves(entity);
            }

            return Result.Success();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> Delete(List<string> ids)
        {
            bool result = await _repository.DeleteByIdsAsync(ids.ToArray());

            if (result)
            {
                ids.ForEach(async item =>
                {
                    result = await _iShelvesPositionService.DeleteEntityByShelves(new ShelvesEntity() { Id = item });
                });
            }

            return Result.Success();
        }

        /// <summary>
        /// 单个详情查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetDetailById([FromQuery] string id)
        {
            return Result.Success().SetData(await _iShelvesService.GetDetailById(id));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public override async Task<Result> Update(ShelvesEntity entity)
        {
            bool result = await _repository.UpdateIgnoreNullAsync(entity);

            if (result)
            {
                await _iShelvesPositionService.UpdateEntityByShelves(entity);
            }

            return Result.Success();
        }

        /// <summary>
        /// 初始化仓库点位信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> InitPointMap()
        {
            // 获取所有x真实坐标值
            // 获取所有y真实坐标值
            List<int?> pointX = await _iEquipmentService.GetAllPointXReal();
            List<int?> pointY = await _iEquipmentService.GetAllPointYReal();

            pointX.AddRange(await _iShelvesPositionService.GetAllPointXReal());
            pointY.AddRange(await _iShelvesPositionService.GetAllPointYReal());

            pointX = pointX.Distinct().ToList();
            pointY = pointY.Distinct().ToList();

            pointX.Sort();
            pointY.Sort();

            await _iPointMapService.ClearTable();

            for (int index = 1; index < pointX.Count; index++)
            {
                PointMapEntity point = new PointMapEntity()
                {
                    point_type = "x",
                    value = index - 0,
                    min_realy = pointX[index - 1],
                    max_realy = pointX[index]
                };
                await _iPointMapService.AddEntity(point);
            }

            for (int index = 1; index < pointY.Count; index++)
            {
                PointMapEntity point = new PointMapEntity()
                {
                    point_type = "y",
                    value = index - 0,
                    min_realy = pointY[index - 1],
                    max_realy = pointY[index]
                };
                await _iPointMapService.AddEntity(point);
            }

            return Result.Success().SetData(new { pointX, pointY });
        }

        /// <summary>
        /// 更新货位信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> UpdateShelvesPosition(ShelvesPositionEntity entity)
        {
            await _iShelvesPositionService.UpdateEntity(entity);

            return Result.Success();
        }
    }
}
