namespace CodeRun.Services.IService.Dtos.Inputs
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdatePasswordInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = null!;
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = null!;
    }
}
