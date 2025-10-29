namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// app注册
    /// </summary>
    public class AppRegisterInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 性别:0-女,1-男
        /// </summary>
        public int Sex { get; set; }
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
}
