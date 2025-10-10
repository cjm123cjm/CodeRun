using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class RoleForMenuRepository : BaseRepository<RoleForMenu>, IRoleForMenuRepository
    {
        protected RoleForMenuRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
