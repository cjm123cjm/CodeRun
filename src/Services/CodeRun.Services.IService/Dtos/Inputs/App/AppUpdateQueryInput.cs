namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    public class AppUpdateQueryInput : PageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? PulishStartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? PulishEndTime { get; set; }
    }
}
