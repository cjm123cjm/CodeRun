namespace CodeRun.Services.IService.Dtos.Outputs.App
{
    /// <summary>
    /// 设备管理dto
    /// </summary>
    public class AppDeviceDto
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long DeviceId { get; set; }
        /// <summary>
        /// 手机品牌
        /// </summary>
        public string DeviceBrand { get; set; } = null!;
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
