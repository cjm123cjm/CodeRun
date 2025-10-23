namespace CodeRun.Services.IService.Dtos.Outputs.Web
{
    public class MenuTreeDto
    {
        /// <summary>
        /// 主键
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

        public List<MenuTreeDto> ChildMenu { get; set; }
    }
}
