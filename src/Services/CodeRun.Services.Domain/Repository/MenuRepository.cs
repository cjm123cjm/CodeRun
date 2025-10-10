using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        protected MenuRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
