using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
