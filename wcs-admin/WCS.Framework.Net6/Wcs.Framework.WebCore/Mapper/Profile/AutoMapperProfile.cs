using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Model.Models;

namespace Wcs.Framework.WebCore.Mapper
{
    public class AutoMapperProfile : Profile
    {
        // 添加你的实体映射关系. 
        public AutoMapperProfile()
        {
            CreateMap<ArticleEntity, ArticleVo>();
            CreateMap<UserEntity, UserVo>();
            CreateMap<EquipmentTypeEntity, EquipmentTypeVo>();
            CreateMap<WarehouseEntity, WarehouseVo>();
            CreateMap<EquipmentEntity, EquipmentVo>();
            CreateMap<AlarmLevelEntity, AlarmLevelVo>();
            CreateMap<ShelvesEntity, ShelvesVo>();
        }
    }


}
