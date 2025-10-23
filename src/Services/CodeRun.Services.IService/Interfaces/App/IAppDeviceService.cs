using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppDeviceService
    {
        /// <summary>
        /// 加载设备列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<List<AppDeviceDto>> LoadAppDeviceListAsync(AppDeviceQueryInput queryInput);
    }
}
