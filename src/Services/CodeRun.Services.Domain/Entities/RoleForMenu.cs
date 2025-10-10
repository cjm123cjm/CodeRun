namespace CodeRun.Services.Domain.Entities
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    public class RoleForMenu
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 0-半选,1-全选
        /// </summary>
        public short CheckType { get; set; }
    }
}
