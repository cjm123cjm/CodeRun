namespace CodeRun.Services.IService.Dtos.Outputs.App
{
    public class AppUpdateDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = null!;
        /// <summary>
        /// 更新描述
        /// </summary>
        public string UpdateDesc { get; set; } = null!;
        /// <summary>
        /// 更新类型:0-全更新,1-局部热更新
        /// </summary>
        public int UpdateType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 0-未发布,1-灰度发布,2-全网发布
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 灰度设备id
        /// </summary>
        public string? GrayscaleDevice { get; set; }
    }
}
