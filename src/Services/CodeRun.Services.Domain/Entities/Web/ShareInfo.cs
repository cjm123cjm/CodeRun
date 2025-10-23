namespace CodeRun.Services.Domain.Entities.Web
{
    /// <summary>
    /// 分享
    /// </summary>
    public class ShareInfo
    {
        /// <summary>
        /// 分享id
        /// </summary>
        public long ShareId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// 0-无封面，1-横幅，2-小图标
        /// </summary>
        public int CoverType { get; set; }
        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverPath { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态：0-未发布 1-已发布
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public long CreatedUserId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatedUserName { get; set; }
        /// <summary>
        /// 阅读数量
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 收藏数量
        /// </summary>
        public int CollectCount { get; set; }
        /// <summary>
        /// 0-内部 1-外部投稿
        /// </summary>
        public int PostUserType { get; set; }
    }
}
