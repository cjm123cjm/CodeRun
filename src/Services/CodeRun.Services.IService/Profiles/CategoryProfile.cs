using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Profiles
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}
