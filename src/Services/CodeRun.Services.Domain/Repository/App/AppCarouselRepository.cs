using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppCarouselRepository : BaseRepository<AppCarousel>, IAppCarouselRepository
    {
        protected AppCarouselRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public Task<int> MaxSortAsync()
        {
            return Query().AsNoTracking().MaxAsync(t => t.Sort);
        }
    }
}
