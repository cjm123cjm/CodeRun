namespace CodeRun.Services.IService.Dtos.Inputs
{
    public class AccountUpdateInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 手机号(唯一)
        /// </summary>
        public string Phone { get; set; } = null!;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;
        /// <summary>
        /// 职位
        /// </summary>
        public string? Position { get; set; }
        /// <summary>
        /// 角色(多个角色用,号分割)
        /// </summary>
        public string? Roles { get; set; }
    }
}
