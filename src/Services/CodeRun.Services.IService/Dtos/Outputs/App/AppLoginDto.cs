namespace CodeRun.Services.IService.Dtos.Outputs.App
{
    public class AppLoginDto
    {
        public long UserId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        public string Email { get; set; } = null!;
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = null!;
    }
}
