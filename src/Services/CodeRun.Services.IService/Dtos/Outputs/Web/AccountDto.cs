namespace CodeRun.Services.IService.Dtos.Outputs.Web
{
    public class AccountDto
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
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 职位
        /// </summary>
        public string? Position { get; set; }
        /// <summary>
        /// 状态 0-禁用 1-启用
        /// </summary>
        public short Status { get; set; }
        /// <summary>
        /// 角色Id(多个角色用,号分割)
        /// </summary>
        public string? Roles { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleNames { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
