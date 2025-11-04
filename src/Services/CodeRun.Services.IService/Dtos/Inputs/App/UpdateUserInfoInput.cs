namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 更新用户信息输入参数
    /// </summary>
    public class UpdateUserInfoInput
    {
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string? OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string? NewPassword { get; set; }
    }
}
