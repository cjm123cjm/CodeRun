namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// app登录输入参数
    /// </summary>
    public class AppLoginInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; } = null!;
        /// <summary>
        /// 设备品牌
        /// </summary>
        public string DeviceBrand { get; set; } = null!;
        /// <summary>
        /// 验证码id
        /// </summary>
        public string CheckCodeKey { get; set; } = null!;
        /// <summary>
        /// 验证码
        /// </summary>
        public string CheckCode { get; set; } = null!;
    }
    /// <summary>
    /// 自动登录
    /// </summary>
    public class AutoLoginInput
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; } = null!;
        /// <summary>
        /// 设备品牌
        /// </summary>
        public string DeviceBrand { get; set; } = null!;
    }
}
