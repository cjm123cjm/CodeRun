using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, Role>().ReverseMap();
            CreateMap<RoleAddOrUpdateInput, Role>().ReverseMap();
        }
    }
}
