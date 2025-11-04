namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class ShowNextDetailInput : PageInput
    {
        /// <summary>
        /// 当前objectId
        /// </summary>
        public long CurrentId { get; set; }
        /// <summary>
        /// 1:上一页,2:下一页,3当前页
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public int CollectType { get; set; }
    }
}
