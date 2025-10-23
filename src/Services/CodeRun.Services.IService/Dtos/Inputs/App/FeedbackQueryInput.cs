namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 反馈查询输入参数
    /// </summary>
    public class FeedbackQueryInput : PageInput
    {
        /// <summary>
        /// 反馈开始时间
        /// </summary>
        public DateTime? FeedbackStartTime { get; set; }
        /// <summary>
        /// 反馈结束时间
        /// </summary>
        public DateTime? FeedbackEndTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? UserName { get; set; }
    }
}
