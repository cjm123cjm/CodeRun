using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

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
