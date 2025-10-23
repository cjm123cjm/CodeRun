namespace CodeRun.Services.Domain.Entities.App
{
    /// <summary>
    /// app轮播图
    /// </summary>
    public class AppCarousel
    {
        /// <summary>
        /// 轮播图id
        /// </summary>
        public long CarouselId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgPath { get; set; } = null!;
        /// <summary>
        /// 0-分享,1-问题,2-考题,3-外部链接
        /// </summary>
        public int ObjectType { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        public string? OuterLink { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
