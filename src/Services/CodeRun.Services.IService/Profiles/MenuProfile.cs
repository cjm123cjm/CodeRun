using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

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
