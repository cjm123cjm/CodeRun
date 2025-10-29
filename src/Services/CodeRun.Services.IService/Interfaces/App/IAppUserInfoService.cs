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

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        Task RegisterAsync(AppRegisterInput registerInput);

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        Task<AppLoginDto> LoginAsync(AppLoginInput loginInput);

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<AppLoginDto> AutoLoginAsync(AutoLoginInput loginInput);
    }
}
