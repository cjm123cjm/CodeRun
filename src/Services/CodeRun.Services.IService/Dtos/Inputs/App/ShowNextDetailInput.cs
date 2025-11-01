namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class ShowNextDetailInput : PageInput
    {
        /// <summary>
        /// 当前objectId
        /// </summary>
        public long CurrentId { get; set; }
        /// <summary>
        /// 0:上一页,1:下一页
        /// </summary>
        public int Type { get; set; }
    }
}
