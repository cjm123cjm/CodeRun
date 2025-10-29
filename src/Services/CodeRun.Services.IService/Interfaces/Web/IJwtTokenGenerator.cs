using CodeRun.Services.IService.Dtos.Outputs.App;
using CodeRun.Services.IService.Dtos.Outputs.Web;

namespace CodeRun.Services.IService.Interfaces.Web
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

        /// <summary>
        /// app生成token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string AppGenerateToken(AppLoginDto account);
    }
}
