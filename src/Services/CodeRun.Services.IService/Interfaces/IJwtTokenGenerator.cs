using CodeRun.Services.IService.Dtos.Outputs;

namespace CodeRun.Services.IService.Interfaces
{
    /// <summary>
    /// 生成token
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="account">账户信息</param>
        /// <param name="permissionCodes">权限信息</param>
        /// <returns></returns>
        string GenerateToken(AccountDto account, List<string> permissionCodes);
    }
}
