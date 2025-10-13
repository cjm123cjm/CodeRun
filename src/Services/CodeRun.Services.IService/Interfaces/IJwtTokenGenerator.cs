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
        /// <param name="userDto"></param>
        /// <returns></returns>
        string GenerateToken(AccountDto account);
    }
}
