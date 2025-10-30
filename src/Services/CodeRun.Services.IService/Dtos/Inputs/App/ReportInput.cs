namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class ReportInput
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public string? DeviceBrand { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; } = null!;
    }
}
