namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    /// <summary>
    /// 分享查询输入参数
    /// </summary>
    public class ShareInfoQueryInput : PageInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 状态：0-未发布 1-已发布
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string? CreatedUserName { get; set; }


        /// <summary>
        /// 当前shareIfoId
        /// </summary>
        public long? CurrentShareInfoId { get; set; }

        /// <summary>
        /// 1-上一页,2-下一页,3-当前页
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 阅读量是否++
        /// </summary>
        public bool ReadCount { get; set; } = false;
    }
}
