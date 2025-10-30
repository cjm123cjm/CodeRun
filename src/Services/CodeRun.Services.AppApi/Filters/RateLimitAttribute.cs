using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.Common.RedisUtil;

namespace CodeRun.Services.AppApi.Filters
{
    /// <summary>
    /// 限流
    /// </summary>
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly int _limit;
        private readonly int _seconds;

        public RateLimitAttribute(int limit, int seconds)
        {
            _limit = limit;
            _seconds = seconds;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_limit == 0)
            {
                await next();
            }
            else
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<RateLimitAttribute>>();

                var clientIp = GetClientIP(context.HttpContext);
                var actionName = context.ActionDescriptor.DisplayName;
                var cacheKey = $"rate_filter_{clientIp}_{actionName}";

                var cacheData = CacheManager.Get<int?>(cacheKey);
                if (cacheData == null)
                {
                    CacheManager.StringIncrementWithExpiry(cacheKey, 1, TimeSpan.FromSeconds(_seconds));
                    await next();
                    return;
                }

                if (cacheData >= _limit)
                {
                    logger?.LogWarning($"IP {clientIp} 在 {actionName} 上触发限流");
                    context.Result = new ObjectResult(new { Message = "请求过于频繁", IsSuccess = false })
                    {
                        StatusCode = 200
                    };
                    return;
                }

                CacheManager.StringIncrementWithExpiry(cacheKey, 1, TimeSpan.FromSeconds(_seconds));
                await next();
            }
        }

        private string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
                ip = context.Connection.RemoteIpAddress?.ToString();
            return ip ?? "unknown";
        }

        private class RequestData
        {
            public int Count { get; set; }
        }
    }
}
