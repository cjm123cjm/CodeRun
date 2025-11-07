using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppUserInfoRepository : BaseRepository<AppUserInfo>, IAppUserInfoRepository
    {
        public AppUserInfoRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
