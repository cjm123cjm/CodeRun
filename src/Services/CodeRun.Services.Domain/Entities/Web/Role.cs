namespace CodeRun.Services.Domain.Entities.Web
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = null!;
        /// <summary>
        /// 角色描述
        /// </summary>
        public string? RoleDesc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; } = null;
    }
}
