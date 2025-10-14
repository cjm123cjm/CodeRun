using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class ShareInfoRepository : BaseRepository<ShareInfo>, IShareInfoRepository
    {
        protected ShareInfoRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
