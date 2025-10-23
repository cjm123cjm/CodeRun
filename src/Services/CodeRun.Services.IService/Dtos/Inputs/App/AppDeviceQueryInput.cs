namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class AppDeviceQueryInput : PageInput
    {
        /// <summary>
        /// 加入日期-开始
        /// </summary>
        public DateTime? JoinTimeStart { get; set; }
        /// <summary>
        /// 加入日期-结束
        /// </summary>
        public DateTime? JoinTimeEnd { get; set; }
        /// <summary>
        /// 最近使用日期-开始
        /// </summary>
        public DateTime? LastUseTimeStart { get; set; }
        /// <summary>
        /// 最近使用日期-结束
        /// </summary>
        public DateTime? LastUseTimeEnd { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string? DeviceBrand { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string? DeviceId { get; set; }
    }
}
