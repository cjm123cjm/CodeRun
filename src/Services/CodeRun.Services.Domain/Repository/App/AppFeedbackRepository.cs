using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppFeedbackRepository : BaseRepository<AppFeedback>, IAppFeedbackRepository
    {
        protected AppFeedbackRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
