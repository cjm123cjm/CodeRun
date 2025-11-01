namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 搜索输入参数
    /// </summary>
    public class SearchInput : PageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; } = null!;
        /// <summary>
        /// 0:八股文,1:分享,2:考题
        /// </summary>
        public int Type { get; set; }
    }
}
