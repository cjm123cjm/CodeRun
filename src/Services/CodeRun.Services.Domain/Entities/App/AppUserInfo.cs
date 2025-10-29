namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// app用户信息
    /// </summary>
    public class AppUserInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; } = null;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 性别:0-女,1-男
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime JoinTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 最后使用的设备id
        /// </summary>
        public string LastUseDeviceId { get; set; }
        /// <summary>
        /// 最后使用的手机品牌
        /// </summary>
        public string? LastUseDeviceBrand { get; set; }
        /// <summary>
        /// 最后登录的设备ip
        /// </summary>
        public string? LastLoginIp { get; set; }
        /// <summary>
        /// 状态:0-禁用,1-启用
        /// </summary>
        public int Status { get; set; }
    }
}
