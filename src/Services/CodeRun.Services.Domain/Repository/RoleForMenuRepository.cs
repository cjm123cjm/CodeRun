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
            return await QueryWhere(t => roleIds.Contains(t.RoleId) && t.CheckType == 1, false).Select(t => t.MenuId).Distinct().ToListAsync();
        }

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenusAsync()
        {
            var menus = await _context.Menus.AsNoTracking().ToListAsync();

            return menus;
        }

        /// <summary>
        /// 根据角色id获取菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenusByRoleIdAsync(params long[] roleIds)
        {
            var menuIds = await QueryWhere(t => roleIds.Contains(t.RoleId), false).Select(t => t.MenuId).Distinct().ToListAsync();

            var menus = await _context.Menus.AsNoTracking().Where(t => menuIds.Contains(t.MenuId)).ToListAsync();

            return menus;
        }
    }
}
