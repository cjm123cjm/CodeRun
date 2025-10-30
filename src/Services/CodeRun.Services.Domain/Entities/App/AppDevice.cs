namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class AppDevice
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; } = null!;
        /// <summary>
        /// 手机品牌
        /// </summary>
        public string? DeviceBrand { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 最后使用时间
        /// </summary>
        public DateTime? LastUseTime { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; } = null!;
    }
}
