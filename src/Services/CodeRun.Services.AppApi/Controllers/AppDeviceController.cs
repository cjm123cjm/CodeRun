using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Interfaces.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeRun.Services.AppApi.Controllers
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
        public async Task<ResponseDto> LoadAppDeviceListAsync(AppDeviceQueryInput queryInput)
        {
            var data = await _appDeviceService.LoadAppDeviceListAsync(queryInput);

            return new ResponseDto(data);
        }
    }
}
