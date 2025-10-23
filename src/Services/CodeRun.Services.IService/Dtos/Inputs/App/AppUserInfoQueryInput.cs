namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 用户管理查询参数
    /// </summary>
    public class AppUserInfoQueryInput : PageInput
    {
        public DateTime? JoinTimeStart { get; set; }
        public DateTime? JoinTimeEnd { get; set; }
        public string? Email { get; set; }
        public string? DeviceId { get; set; }
    }
}
