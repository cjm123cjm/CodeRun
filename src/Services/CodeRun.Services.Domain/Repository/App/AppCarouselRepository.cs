using CodeRun.Services.Domain.Entities.App;
using CodeRun.Services.Domain.IRepository.App;
using CodeRun.Services.Domain.Repository.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.App
{
    public class AppCarouselRepository : BaseRepository<AppCarousel>, IAppCarouselRepository
    {
        public AppCarouselRepository(CodeRunDbContext context) : base(context)
        {
        }

        public async Task<int> MaxSortAsync()
        {
            int count = await Query().AsNoTracking().CountAsync();
            return count == 0 ? 0 : (await Query().AsNoTracking().MaxAsync(t => t.Sort));
        }
    }
}
