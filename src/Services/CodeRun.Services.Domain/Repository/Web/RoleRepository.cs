using CodeRun.Services.Domain.Entities.Web;
using CodeRun.Services.Domain.IRepository.Web;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository.Web
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(CodeRunDbContext context) : base(context)
        {
        }
    }
}
