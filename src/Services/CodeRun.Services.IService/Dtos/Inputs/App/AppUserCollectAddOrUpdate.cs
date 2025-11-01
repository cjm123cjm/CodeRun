namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 添加/取消收藏输入参数
    /// </summary>
    public class AppUserCollectAddOrUpdate
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int CollectType { get; set; }
        /// <summary>
        /// 0:添加,1:取消
        /// </summary>
        public int AddOrCancel { get; set; }
    }
}
