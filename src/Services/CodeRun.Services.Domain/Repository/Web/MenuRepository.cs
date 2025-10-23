using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        protected MenuRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
