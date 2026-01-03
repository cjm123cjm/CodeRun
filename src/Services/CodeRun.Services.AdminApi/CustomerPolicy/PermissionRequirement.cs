using CodeRun.Services.IService.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CodeRun.Services.AdminApi.CustomerPolicy
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionCodeEnum[] PermissionCodes { get; }

        public PermissionRequirement(PermissionCodeEnum[] permissionCodes)
        {
            PermissionCodes = permissionCodes;
        }
    }

    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 检查权限要求列表是否包含 "No_Permission"
            // 如果包含，则直接授权通过，不进行任何验证
            if (requirement.PermissionCodes.Any(p =>
                p.ToString().Equals("No_Permission", StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // 检查用户是否认证
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // 获取用户的所有权限
            var userPermissions = context.User.FindAll("Permission")
                .Select(c => c.Value)
                .ToList();

            // 检查用户是否拥有所需的任一权限
            var hasPermission = requirement.PermissionCodes
                .Any(requiredPermission => userPermissions.Contains(requiredPermission.ToString()));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
