using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Interfaces.Web
{
    public interface IMenuService
    {
        /// <summary>
        /// 加载菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<MenuTreeDto>> LoadMenuTreeAsync();

        /// <summary>
        /// 添加/修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveMenuAsync(MenuAddOrUpdateInput input);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单id</param>
        /// <returns></returns>
        Task DeletedMenuAsync(long menuId);
    }
}
