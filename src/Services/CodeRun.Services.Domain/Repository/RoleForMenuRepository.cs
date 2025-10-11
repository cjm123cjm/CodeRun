using CodeRun.Services.Domain.Entities;
using CodeRun.Services.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeRun.Services.Domain.Repository
{
    public class RoleForMenuRepository : BaseRepository<RoleForMenu>, IRoleForMenuRepository
    {
        protected RoleForMenuRepository(CodeRunDbContext context, ILogger logger) : base(context, logger)
        {
        }

        /// <summary>
        /// 根据角色id获取菜单id
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<List<long>> GetMenuIdsByRoleIdAsync(params long[] roleIds)
        {
            return await QueryWhere(t => roleIds.Contains(t.RoleId) && t.CheckType == 1, false).Select(t => t.MenuId).ToListAsync();
        }
    }
}
