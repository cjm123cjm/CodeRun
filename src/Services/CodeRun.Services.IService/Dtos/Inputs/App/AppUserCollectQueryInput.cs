namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 用户收藏查询输入参数
    /// </summary>
    public class AppUserCollectQueryInput : PageInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 收藏类别
        /// </summary>
        public int CollectType { get; set; }
    }
}
