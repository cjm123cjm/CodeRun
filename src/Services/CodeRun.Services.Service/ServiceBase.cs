using AutoMapper;
using CodeRun.Services.Common.Snowflake;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using CodeRun.Services.IService.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeRun.Services.Service
{
    public class ServiceBase
    {
        protected long LoginUserId { get; set; }
        protected string LoginUserName { get; set; }
        public string Roles { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        protected IMapper ObjectMapper { get; set; }
        protected string ServerUrl { get; set; }
        protected IdWorker SnowIdWorker { get; set; }
        protected FolderPath FolderPath { get; set; }
        protected string UserIp { get; set; }
        public ServiceBase()
        {
            var httpContext = LocationStorage.Instance.GetService<IHttpContextAccessor>()!;

            if (httpContext.HttpContext != null && httpContext.HttpContext.User != null && httpContext.HttpContext.User.Identity != null)
            {
                if (httpContext.HttpContext!.User!.Identity!.IsAuthenticated)
                {
                    LoginUserId = Convert.ToInt64(httpContext.HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
                    Roles = httpContext.HttpContext.User.Claims.First(t => t.Type == "Roles").Value;
                    LoginUserName = httpContext.HttpContext.User.Claims.First(t => t.Type == "UserName").Value.ToString();
                    Email = httpContext.HttpContext.User.Claims.First(t => t.Type == "NickName").Value.ToString();
                    IsAdmin = Convert.ToBoolean(httpContext.HttpContext.User.Claims.First(t => t.Type == "IsAdmin").Value.ToString());
                }
                ServerUrl = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}";

                UserIp = GetClientIPAddress(httpContext.HttpContext);
            }

            ObjectMapper = LocationStorage.Instance.GetService<IMapper>()!;

            SnowIdWorker = SnowflakeUtil.CreateIdWorker();

            FolderPath = LocationStorage.Instance.GetRequiredService<IOptions<FolderPath>>().Value;
        }

        protected List<MenuTreeDto> BuildTreeMenu(List<MenuTreeDto> menus, long parentId)
        {
            List<MenuTreeDto> menuTrees = new List<MenuTreeDto>();
            foreach (var item in menus)
            {
                if (item.ParentId == parentId)
                {
                    item.ChildMenu.AddRange(BuildTreeMenu(menus, item.MenuId));
                    menuTrees.Add(item);
                }
            }

            return menuTrees;
        }
        protected string GetClientIPAddress(HttpContext context)
        {
            try
            {
                // 检查头信息（处理代理和负载均衡情况）
                var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (!string.IsNullOrEmpty(ip))
                {
                    // X-Forwarded-For 可能包含多个IP，取第一个
                    ip = ip.Split(',')[0].Trim();
                }

                // 如果 X-Forwarded-For 为空，尝试其他头信息
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Request.Headers["X-Real-IP"].FirstOrDefault();
                }

                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Request.Headers["X-Client-IP"].FirstOrDefault();
                }

                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault(); // Cloudflare
                }

                // 最后使用 RemoteIpAddress
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Connection.RemoteIpAddress?.ToString();

                    // 处理 IPv6 本地地址
                    if (ip == "::1")
                    {
                        ip = "127.0.0.1";
                    }
                }

                return ip ?? "未知IP";
            }
            catch (Exception ex)
            {
                // 记录日志
                return $"获取IP失败: {ex.Message}";
            }
        }
    }
}
