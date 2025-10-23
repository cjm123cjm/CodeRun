using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.Web;
using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Enums;
using CodeRun.Services.IService.Interfaces.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 添加(角色+角色权限)/修改(角色)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_role_edit)]
        public async Task<ResponseDto> SaveRole(RoleAddOrUpdateInput input)
        {
            await _roleService.SaveRoleAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_role_edit)]
        public async Task<ResponseDto> SaveRoleMenu(RoleAddOrUpdateInput input)
        {
            await _roleService.SaveRoleMenuAsync(input);

            return new ResponseDto();
        }

        /// <summary>
        /// 加载角色列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.settings_role_list)]
        public async Task<ResponseDto> LoadRoleList(RoleQueryInput queryInput)
        {
            var data = await _roleService.LoadRoleListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 根据角色id查询菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.settings_role_list)]
        public async Task<ResponseDto> RoleMenuByRoleId(long roleId)
        {
            var data = await _roleService.RoleMenuByRoleIdAsync(roleId);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.settings_role_del)]
        public async Task<ResponseDto> DeletedRole(long roleId)
        {
            await _roleService.DeletedRoleAsync(roleId);

            return new ResponseDto();
        }
    }
}
