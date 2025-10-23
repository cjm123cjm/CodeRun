namespace CodeRun.Services.IService.Dtos.Inputs.App
{
    /// <summary>
    /// 修改用户状态输入参数
    /// </summary>
    public class UpdateStatusAppUserInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int AppUserId { get; set; }
        /// <summary>
        /// 0-禁用,1-启用
        /// </summary>
        public int Status { get; set; }
    }
}
