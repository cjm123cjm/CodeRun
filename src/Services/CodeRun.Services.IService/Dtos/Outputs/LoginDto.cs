namespace CodeRun.Services.IService.Dtos.Outputs
{
    public class LoginDto
    {
        /// <summary>
        /// 账户信息
        /// </summary>
        public AccountDto Account { get; set; } = null!;

        /// <summary>
        /// 菜单
        /// </summary>
        public List<MenuTreeDto> Menus { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public List<string> PermissionCodes { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = null!;
    }
}
