namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// app发布输入参数
    /// </summary>
    public class PostAppUpdateInput
    {
        /// <summary>
        /// id
        /// </summary>
        public long AppUpdateId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 灰度发布设备id
        /// </summary>
        public string? GrayscaleDevice { get; set; }
    }
}
