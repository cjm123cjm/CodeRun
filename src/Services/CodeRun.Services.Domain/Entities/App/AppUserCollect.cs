namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    public class AppUserCollect
    {
        /// <summary>
        /// id
        /// </summary>
        public long CollectId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 主题id 问题id 考题id 分享文章id 
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 0:分享收藏,1:问题收藏,2:考题收藏
        /// </summary>
        public int CollectType { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime CollectTime { get; set; }
    }
}
