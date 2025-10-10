namespace CodeRun.Services.IService.Dtos.Outputs
{
    public class LoginDto
    {
        /// <summary>
        /// 账户信息
        /// </summary>
        public AccountDto Account { get; set; } = null!;

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = null!;
    }
}
