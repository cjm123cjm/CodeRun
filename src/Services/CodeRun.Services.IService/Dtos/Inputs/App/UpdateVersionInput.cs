namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class UpdateVersionInput
    {
        /// <summary>
        /// app版本
        /// </summary>
        public string? AppVersion { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; } = null!;
    }
}
