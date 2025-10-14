using System.ComponentModel;

namespace CodeRun.Services.IService.Enums
{
    /// <summary>
    /// 菜单权限编码
    /// </summary>
    public enum PermissionCodeEnum
    {
        [Description("不校验权限")]
        No_Permission,

        [Description("首页")]
        home,

        [Description("系统设置")]
        settings,
        [Description("菜单列表")]
        settings_menu_list,
        [Description("新增/修改")]
        settings_menu_edit,
        [Description("删除")]
        settings_menu_del,

        [Description("角色列表")]
        settings_role_list,
        [Description("角色新增/修改")]
        settings_role_edit,
        [Description("角色删除")]
        settings_role_del,

        [Description("用户列表")]
        settings_account_list,
        [Description("新增/修改")]
        settings_account_edit,
        [Description("删除")]
        settings_account_del,
        [Description("修改密码")]
        settings_account_update_password,
        [Description("启用/禁用")]
        settings_account_op_status,

        [Description("内容管理")]
        content,
        [Description("分类列表")]
        category_list,
        [Description("新增/修改")]
        category_edit,
        [Description("删除")]
        category_del,

        [Description("问题列表")]
        question_list,
        [Description("新增/修改")]
        question_edit,
        [Description("导入")]
        question_import,
        [Description("发布")]
        question_post,
        [Description("删除")]
        question_del,
        [Description("批量删除")]
        question_del_batch,

        [Description("考试列表")]
        exam_question_list,
        [Description("新增/修改")]
        exam_question_edit,
        [Description("导入")]
        exam_question_import,
        [Description("发布")]
        exam_question_post,
        [Description("删除")]
        exam_question_del,
        [Description("批量删除")]
        exam_question_del_batch,

        [Description("经验分享列表")]
        share_list,
        [Description("新增/修改")]
        share_edit,
        [Description("发布/取消发布")]
        share_post,
        [Description("删除")]
        share_del,
        [Description("批量删除")]
        share_del_batch,

        [Description("应用发布列表")]
        app_update_list,
        [Description("新增/修改/删除")]
        app_update_edit,
        [Description("应用发布")]
        app_update_post,


        [Description("轮播图列表")]
        app_carousel_list,
        [Description("轮播图新增/修改/删除")]
        app_carousel_edit,

        [Description("APP用户")]
        app_user_list,
        [Description("APP用户编辑")]
        app_user_edit,

        [Description("APP用户设备")]
        app_device_list
    }
}
