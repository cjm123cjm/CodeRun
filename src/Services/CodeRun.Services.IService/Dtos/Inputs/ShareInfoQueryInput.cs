namespace CodeRun.Services.IService.Dtos.Inputs
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
        /// 0-上一页,1-下一页
        /// </summary>
        public int Type { get; set; }
    }
}
