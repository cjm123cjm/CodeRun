namespace CodeRun.Services.IService.Dtos.Outputs
{
    public class RoleDto
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

        /// <summary>
        /// 菜单ids
        /// </summary>
        public List<long> MenuIds { get; set; }
    }
}
