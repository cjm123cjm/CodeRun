using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Profiles
{
    public class ShareInfoProfile : Profile
    {
        public ShareInfoProfile()
        {
            CreateMap<ShareInfoDto, ShareInfo>().ReverseMap();
            CreateMap<ShareInfoAddOrUpdateInput, ShareInfo>().ReverseMap();
        }
    }
}
