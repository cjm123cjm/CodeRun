using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        protected AccountRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
