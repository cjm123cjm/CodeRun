namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// app反馈内容
    /// </summary>
    public class AppFeedback
    {
        /// <summary>
        /// 反馈id
        /// </summary>
        public long FeedbackId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; } = null!;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 父级id
        /// </summary>
        public long FeedbackParentId { get; set; }
        /// <summary>
        /// 状态:0-未回复,1-已回复
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 0-访客,1-管理员
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 访客最后发送时间
        /// </summary>
        public DateTime? ClientLastSendTime { get; set; }
    }
}
