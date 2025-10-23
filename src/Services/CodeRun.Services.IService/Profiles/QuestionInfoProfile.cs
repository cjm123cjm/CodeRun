using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Profiles
{
    public class QuestionInfoProfile : Profile
    {
        public QuestionInfoProfile()
        {
            CreateMap<QuestionInfoAddOrUpdateInput, QuestionInfo>().ReverseMap();
            CreateMap<QuestionInfoDto, QuestionInfo>().ReverseMap();
        }
    }
}
