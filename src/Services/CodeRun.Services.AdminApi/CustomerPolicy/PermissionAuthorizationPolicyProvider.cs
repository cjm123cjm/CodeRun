using CodeRun.Services.IService.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.AdminApi.CustomerPolicy
{
    public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() =>
            _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // 检查是否是权限策略
            if (policyName.StartsWith("HasPermission:", StringComparison.OrdinalIgnoreCase))
            {
                // 解析权限代码
                var permissionCodes = policyName.Substring("HasPermission:".Length)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);


                PermissionCodeEnum[] permissionEnums = permissionCodes
                                                        .Select(p => Enum.Parse<PermissionCodeEnum>(p))
                                                        .ToArray();

                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(permissionEnums));
                return Task.FromResult(policy.Build());
            }

            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
