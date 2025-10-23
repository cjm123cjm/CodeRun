using Autofac;
using Autofac.Extras.DynamicProxy;
using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.Domain.Repository.Web;
using CodeRun.Services.IService.Interfaces.Web;
using CodeRun.Services.Service.Implements.Web;
using System.Reflection;

namespace CodeRun.Services.AdminApi.Extensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtTokenGenerator>().As<IJwtTokenGenerator>();

            var aopType = new List<Type> { typeof(ServiceAop) };
            builder.RegisterType<ServiceAop>();

            //获取 Service.dll 程序集服务,并注册
            builder.RegisterAssemblyTypes(Assembly.Load("CodeRun.Services.IService"), Assembly.Load("CodeRun.Services.Service"))
                .Where(a => a.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopType.ToArray());

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)); //注册仓储

            //获取 Repository.dll 程序集服务,并注册
            builder.RegisterAssemblyTypes(Assembly.Load("CodeRun.Services.Domain"))
                .Where(a => a.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }
}
