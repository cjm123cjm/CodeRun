using AutoMapper;
using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDto, Account>().ReverseMap();
            CreateMap<AccountAddInput, Account>().ReverseMap();
            CreateMap<AccountUpdateInput, Account>().ReverseMap();
        }
    }
}
