using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 加载完成菜单树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.settings_menu_list)]
        public async Task<ResponseDto> LoadMenuTree()
        {
            var data = await _menuService.LoadMenuTreeAsync();

            return new ResponseDto(data);
        }

        /// <summary>
        /// 添加/修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_menu_edit)]
        public async Task<ResponseDto> SaveMenu(MenuAddOrUpdateInput input)
        {
            await _menuService.SaveMenuAsync(input);
            return new ResponseDto();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单id</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_menu_del)]
        public async Task<ResponseDto> DeletedMenu(long menuId)
        {
            await _menuService.DeletedMenuAsync(menuId);
            return new ResponseDto();
        }
    }
}
