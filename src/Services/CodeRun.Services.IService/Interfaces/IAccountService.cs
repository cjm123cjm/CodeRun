using CodeRun.Services.IService.Dtos;
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

        /// <summary>
        /// 加载用户数据
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PageDto<AccountDto>> LoadAccountListAsync(AccountQueryInput queryInput);

        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAccountAsync(AccountAddInput input);

        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAccountAsync(AccountUpdateInput input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdatePasswordAsync(UpdatePasswordInput input);

        /// <summary>
        /// 修改账户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAccountStatusAsync(UpdateAccountStatusInput input);
    }
}
