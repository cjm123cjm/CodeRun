namespace CodeRun.Services.IService.Dtos.Inputs
{
    /// <summary>
    /// 角色添加/修改
    /// </summary>
    public class RoleAddOrUpdateInput
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
        /// 菜单ids
        /// </summary>
        public string MenuIds { get; set; }
        /// <summary>
        /// 全选还是半选
        /// </summary>
        public string HalfMenuIds { get; set; }
    }
}
