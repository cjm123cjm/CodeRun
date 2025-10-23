using AutoMapper;
using CodeRun.Services.Common.Snowflake;
using CodeRun.Services.IService.Dtos.Outputs.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CodeRun.Services.Service
{
    public class ServiceBase
    {
        protected long LoginUserId { get; set; }
        protected string LoginUserName { get; set; }
        public string Roles { get; set; }
        public bool IsAdmin { get; set; }
        protected IMapper ObjectMapper { get; set; }
        protected string ServerUrl { get; set; }
        protected IdWorker SnowIdWorker { get; set; }

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
                    IsAdmin = Convert.ToBoolean(httpContext.HttpContext.User.Claims.First(t => t.Type == "IsAdmin").Value.ToString());
                }
                ServerUrl = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}";
            }

            ObjectMapper = LocationStorage.Instance.GetService<IMapper>()!;

            SnowIdWorker = SnowflakeUtil.CreateIdWorker();
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
    }
}
