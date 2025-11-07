using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppDeviceRepository : BaseRepository<AppDevice>, IAppDeviceRepository
    {
        public AppDeviceRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
