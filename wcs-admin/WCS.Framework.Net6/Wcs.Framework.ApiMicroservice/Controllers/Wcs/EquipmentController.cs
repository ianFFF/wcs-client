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
    public class EquipmentController : BaseSimpleCrudController<EquipmentEntity>
    {
        private readonly IEquipmentService _iEquipmentService;
        private readonly IEquipmentTypeService _iEquipmentTypeService;
        private readonly IWarehouseService _iWarehouseService;
        private readonly IShelvesService _iShelvesService;
        private readonly IMapper _mapper;

        public EquipmentController(
            ILogger<EquipmentEntity> logger,
            IEquipmentService iEquipmentService,
            IEquipmentTypeService iEquipmentTypeService,
            IWarehouseService iWarehouseService,
            IShelvesService iShelvesService,
            IMapper mapper
        ) : base(logger, iEquipmentService)
        {
            this._iEquipmentService = iEquipmentService;
            this._iEquipmentTypeService = iEquipmentTypeService;
            this._iWarehouseService = iWarehouseService;
            this._iShelvesService = iShelvesService;
            this._mapper = mapper;
        }


        /// <summary>
        /// 获取所有设备列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetAllEquipments([FromQuery] string? type)
        {
            var equipmentData = await _iEquipmentService.SelctList(type);

            return Result.Success().SetData(
                new
                {
                    equipments = _mapper.Map<List<EquipmentVo>>(equipmentData.Data)
                }
            );
        }

        /// <summary>
        /// 手工操作获取所有设备
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetAllEquipmentsManual([FromQuery] string? type, [FromQuery] string? shelves_id)
        {

            List<EquipmentEntity> equipmentData = new List<EquipmentEntity>();
            string? relation_object_id = "";

            if (!string.IsNullOrEmpty(shelves_id))
            {
                ShelvesEntity shelves = await _iShelvesService.GetDetailById(shelves_id);
                relation_object_id = shelves.relation_object_id;
            }

            equipmentData = await _iEquipmentService.SelctListForManual(type, relation_object_id);

            return Result.Success().SetData(
                new
                {
                    equipments = _mapper.Map<List<EquipmentVo>>(equipmentData)
                }
            );
        }

        /// <summary>
        /// 获取下拉条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetOptions()
        {
            var equipmentTypesData = await _iEquipmentTypeService.SelctList();

            var wareHouseData = await _iWarehouseService.SelctList();

            return Result.Success().SetData(
                new
                {
                    equipmentTypes = _mapper.Map<List<EquipmentTypeVo>>(equipmentTypesData.Data),
                    wareHouse = _mapper.Map<List<WarehouseVo>>(wareHouseData.Data)
                }
            );
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> PageList([FromQuery] EquipmentEntity entity, [FromQuery] PageParModel page)
        {
            var pageData = await _iEquipmentService.SelctPageList(entity, page);

            return Result.Success().SetData(new PageModel() { Data = pageData.Data, Total = pageData.Total });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<Result> Add(EquipmentEntity entity)
        {
            return Result.Success().SetData(await _iEquipmentService.AddEntity(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> Delete(List<string> ids)
        {
            return Result.Success().SetStatus(await _repository.DeleteByIdsAsync(ids.ToArray()));
        }

        /// <summary>
        /// 单个详情查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> GetDetailById([FromQuery] string id)
        {
            return Result.Success().SetData(await _iEquipmentService.GetDetailById(id));
        }
    }
}
