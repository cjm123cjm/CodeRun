using AutoMapper;
using CodeRun.Services.Domain.Entities;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

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
