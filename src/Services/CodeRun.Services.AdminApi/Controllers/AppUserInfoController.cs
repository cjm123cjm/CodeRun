using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;

namespace CodeRun.Services.AdminApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppUserInfoController : ControllerBase
    {
        private readonly IAppUserInfoService _appUserInfoService;

        public AppUserInfoController(IAppUserInfoService appUserInfoService)
        {
            _appUserInfoService = appUserInfoService;
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_user_list)]
        public async Task<ResponseDto> LoadAppUserInfoList(AppUserInfoQueryInput queryInput)
        {
            var data = await _appUserInfoService.LoadAppUserInfoListAsync(queryInput);

            return new ResponseDto(data);
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(PermissionCodeEnum.app_user_edit)]
        public async Task<ResponseDto> UpdateStatusAppUserInfo(UpdateStatusAppUserInput update)
        {
            await _appUserInfoService.UpdateStatusAppUserInfoAsync(update);

            return new ResponseDto();
        }
    }
}
