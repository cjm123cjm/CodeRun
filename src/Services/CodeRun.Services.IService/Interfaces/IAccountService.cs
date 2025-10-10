using CodeRun.Services.IService.Dtos.Inputs;
using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<LoginDto> LoginAsync(LoginInput loginInput);
    }
}
