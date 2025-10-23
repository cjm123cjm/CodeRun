namespace CodeRun.Services.IService.Dtos.Inputs.Web
{
    /// <summary>
    /// 菜单添加/修改输入参数
    /// </summary>
    public class MenuAddOrUpdateInput
    {
        /// <summary>
        /// 0-添加，1-修改
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = null!;

        /// <summary>
        /// 菜单类型：0-菜单,1-按钮
        /// </summary>
        public short MenuType { get; set; }

        /// <summary>
        /// 菜单跳转地址
        /// </summary>
        public string? MenuUrl { get; set; }

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string? PermissionCode { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }
}
