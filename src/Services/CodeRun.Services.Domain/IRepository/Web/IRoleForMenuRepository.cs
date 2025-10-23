using CodeRun.Services.Domain.Entities.Web;

namespace CodeRun.Services.Domain.IRepository.Web
{
    public interface IRoleForMenuRepository : IBaseRepository<RoleForMenu>
    {
        /// <summary>
        /// 根据角色id获取菜单id
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<long>> GetMenuIdsByRoleIdAsync(params long[] roleIds);

        /// <summary>
        /// 根据角色id获取菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<Menu>> GetMenusByRoleIdAsync(params long[] roleIds);

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetMenusAsync();
    }
}
