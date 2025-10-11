using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 添加(角色+角色权限)/修改(角色)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveRoleAsync(RoleAddOrUpdateInput input);

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveRoleMenuAsync(RoleAddOrUpdateInput input);

        /// <summary>
        /// 加载角色列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<List<RoleDto>> LoadRoleList(RoleQueryInput queryInput);

        /// <summary>
        /// 根据角色id查询菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<RoleDto> RoleMenuByRoleIdAsync(long roleId);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task DeletedRoleAsync(long roleId);
    }
}
