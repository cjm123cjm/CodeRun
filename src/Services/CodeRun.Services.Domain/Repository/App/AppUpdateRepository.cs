using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppUpdateRepository : BaseRepository<AppUpdate>, IAppUpdateRepository
    {
        protected AppUpdateRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
