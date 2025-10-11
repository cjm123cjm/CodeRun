using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 加载菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        public async Task<ResponseDto> DeletedMenu(long menuId)
        {
            await _menuService.DeletedMenuAsync(menuId);
            return new ResponseDto();
        }
    }
}
