namespace CodeRun.Services.IService.Dtos.Inputs
{
    /// <summary>
    /// 角色查询输入参数
    /// </summary>
    public class RoleQueryInput
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string? RoleDesc { get; set; }
    }
}
