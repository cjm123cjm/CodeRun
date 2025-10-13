using Autofac.Extensions.DependencyInjection;
using Autofac;
using CodeRun.Services.AdminApi.Extensions;
using CodeRun.Services.AdminApi.Filters;
using CodeRun.Services.AdminApi.Middlewares;
using CodeRun.Services.Service;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using CodeRun.Services.IService.Dtos;
using Microsoft.EntityFrameworkCore;
using CodeRun.Services.Domain;
using Serilog;
using IGeekFan.AspNetCore.Knife4jUI;

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
    // ��� long ���͵��Զ���ת��
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

//swagger���jwt��֤
builder.AddSwaggerAuth();

//��֤
builder.AddAuthetication();

// �����������
builder.AddCustomerCors();

//AutoMapper
builder.Services.AddAutoMapper(p =>
{
    p.AddMaps("CodeRun.Services.IService");
});

builder.Services.AddHttpContextAccessor();

//ģ����֤
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

//ע��dbcontext
builder.Services.AddDbContext<CodeRunDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"), new MySqlServerVersion("5.7"));
});

//������־serilog
builder.Host.UseSerilog((context, logger) =>
{
    //Serilog��ȡ����
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseKnife4UI();
}
LocationStorage.Instance = app.Services;

app.UseErrorHandling();

app.UseCors("CodeRun.Client");

app.UseSession();

//��֤
app.UseAuthentication();
//��Ȩ
app.UseAuthorization();

app.MapControllers();
app.Run();