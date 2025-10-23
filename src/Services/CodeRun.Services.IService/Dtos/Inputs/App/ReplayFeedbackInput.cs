namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 反馈回复输入参数
    /// </summary>
    public class ReplayFeedbackInput
    {
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; } = null!;
        /// <summary>
        /// 反馈id
        /// </summary>
        public long FeedbackId { get; set; }
    }
}
