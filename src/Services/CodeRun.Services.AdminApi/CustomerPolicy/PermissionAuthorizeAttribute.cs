using CodeRun.Services.IService.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CodeRun.Services.AdminApi.CustomerPolicy
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(PermissionCodeEnum permissionCode)
        {
            Policy = $"HasPermission:{permissionCode}";
        }

        public PermissionAuthorizeAttribute(params PermissionCodeEnum[] permissionCodes)
        {
            Policy = $"HasPermission:{string.Join(",", permissionCodes.ToString())}";
        }
    }
}
