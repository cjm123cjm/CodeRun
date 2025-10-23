using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<MenuTreeDto, Menu>().ReverseMap();
            CreateMap<MenuAddOrUpdateInput, Menu>().ReverseMap();
        }
    }
}
