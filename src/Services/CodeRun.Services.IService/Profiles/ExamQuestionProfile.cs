using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Profiles
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            CreateMap<ExamQuestion, ExamQuestionDto>().ReverseMap();
            CreateMap<ExamQuestionAddOrUpdateInput, ExamQuestion>().ReverseMap();
            CreateMap<ExamQuestionItemDto, ExamQuestionItem>().ReverseMap();
        }
    }
}
