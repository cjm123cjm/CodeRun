using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

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
