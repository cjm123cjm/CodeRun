using AutoMapper;
using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Profiles
{
    /// <summary>
    /// app
    /// </summary>
    public class AppProfile : Profile
    {
        /// <summary>
        /// 配置
        /// </summary>
        public AppProfile()
        {
            CreateMap<AppCarousel, AppCarouselAddOrUpdateInput>().ReverseMap();
            CreateMap<AppCarouselDto, AppCarousel>().ReverseMap();
            CreateMap<AppUpdate,AppUpdateDto>().ReverseMap();
            CreateMap<AppUpdateAddOrUpdateInput,AppUpdate>().ReverseMap();
        }
    }
}
