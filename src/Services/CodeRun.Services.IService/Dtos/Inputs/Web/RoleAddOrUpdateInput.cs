namespace CodeRun.Services.IService.Dtos.Inputs.Web
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
        /// 按钮ids
        /// </summary>
        public string? MenuIds { get; set; }
        /// <summary>
        /// 半选Ids
        /// </summary>
        public string? HalfMenuIds { get; set; }
    }
}
