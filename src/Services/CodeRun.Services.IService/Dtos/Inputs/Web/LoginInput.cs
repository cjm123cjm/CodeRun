namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    public class LoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// 验证码key
        /// </summary>
        public string? CodeKey { get; set; } = null!;

        /// <summary>
        /// 验证码
        /// </summary>
        public string? Code { get; set; } = null!;
    }
}
