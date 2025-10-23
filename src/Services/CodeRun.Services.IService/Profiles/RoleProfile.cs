using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

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
