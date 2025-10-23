using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.AdminApi.CustomerPolicy;
using CodeRun.Services.IService.Enums;

namespace CodeRun.Services.AdminApi.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppDeviceController : ControllerBase
    {
        private readonly IAppDeviceService _appDeviceService;

        public AppDeviceController(IAppDeviceService appDeviceService)
        {
            _appDeviceService = appDeviceService;
        }
        /// <summary>
        /// 加载设备列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize(PermissionCodeEnum.app_device_list)]
        public async Task<ResponseDto> LoadAppDeviceList(AppDeviceQueryInput queryInput)
        {
            var data = await _appDeviceService.LoadAppDeviceListAsync(queryInput);

            return new ResponseDto(data);
        }
    }
}
