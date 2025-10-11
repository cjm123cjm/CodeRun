namespace CodeRun.Services.IService.Dtos.Inputs
{
    public class AccountQueryInput : PageInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
