namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    /// <summary>
    /// 添加/修改分享输入参数
    /// </summary>
    public class ShareInfoAddOrUpdateInput
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
    }
}
