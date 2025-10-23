using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

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
