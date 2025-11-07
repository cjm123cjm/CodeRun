using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class ShareInfoRepository : BaseRepository<ShareInfo>, IShareInfoRepository
    {
        public ShareInfoRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
