using Autofac.Extensions.DependencyInjection;
using Autofac;
using CodeRun.Services.AppApi.Extensions;
using CodeRun.Services.AppApi.Filters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.Domain;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CodeRun.Services.IService.Options;
using CodeRun.Services.Service;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using CodeRun.Services.AppApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<TokenActionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    };
    // 针对 long 类型的自定义转换
    options.SerializerSettings.Converters.Add(new LongToStringConverterNewtonsoft());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModuleRegister>();
    });

//swagger添加jwt认证
builder.AddSwaggerAuth();

//认证
builder.AddAuthetication();

// 配置允许跨域
builder.AddCustomerCors();

//AutoMapper
builder.Services.AddAutoMapper(p =>
{
    p.AddMaps("CodeRun.Services.IService");
});

builder.Services.AddHttpContextAccessor();

//模型验证
builder.Services.AddOptions().Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorInfo = new ValidationProblemDetails(context.ModelState).Errors
             .Select(t => $"{t.Key}:{string.Join(",", t.Value)}");
        return new OkObjectResult(new ResponseDto
        {
            Code = 200,
            IsSuccess = false,
            Message = string.Join("\r\n", errorInfo)
        });
    };
});

//redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("redis");
    options.InstanceName = "coderun_";
});

//注入dbcontext
builder.Services.AddDbContext<CodeRunDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"), new MySqlServerVersion("5.7"));
});

//配置日志serilog
builder.Host.UseSerilog((context, logger) =>
{
    //Serilog读取配置
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

//配置上传文件位置
var folderPathOption = builder.Configuration.GetSection("FolderPath");
builder.Services.Configure<FolderPath>(folderPathOption);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseKnife4UI();
}
LocationStorage.Instance = app.Services;

app.UseErrorHandling();

app.UseCors("CodeRun.Client");
//静态文件
var path = app.Services.GetRequiredService<IOptions<FolderPath>>();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(path.Value.PhysicalPath)),
    RequestPath = path.Value.virtualPath,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});

//认证
app.UseAuthentication();
//授权
app.UseAuthorization();

app.MapControllers();
app.Run();
