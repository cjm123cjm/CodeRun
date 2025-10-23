using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Dtos.Inputs.App;
using CodeRun.Services.IService.Dtos.Outputs.App;

namespace CodeRun.Services.IService.Interfaces.App
{
    public interface IAppUserInfoService
    {
        /// <summary>
        /// 加载用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<AppUserInfoDto>> LoadAppUserInfoListAsync(AppUserInfoQueryInput queryInput);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task UpdateStatusAppUserInfoAsync(UpdateStatusAppUserInput update);
    }
}
