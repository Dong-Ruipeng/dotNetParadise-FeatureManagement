using dotNetParadise_FeatureManagement.Filters;
using dotNetParadise_FeatureManagement.MiddleWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("appsettings.json", true);
var services = builder.Services;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddProblemDetails();
services.AddHttpContextAccessor();
//.NET 功能管理器是通过框架的本机配置系统配置的
//默认情况下，功能管理器从 .NET 配置数据的 "FeatureManagement" 节检索功能标志配置
services.AddFeatureManagement()
    .AddFeatureFilter<AuthenticatedGroupFilter>();

//service.AddFeatureManagement(builder.Configuration.GetSection("CustomFeatureManagement"));
//.AddFeatureFilter<PercentageFilter>();

//功能管理注册Scoped作用域
//service.AddScopedFeatureManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();
app.UseHttpsRedirection();

//测试中间件的功能开启
app.UseMiddlewareForFeature<FeatureMiddleWare>("featureMiddleWare");


app.MapGet("/sayHello", async Task<IResult> ([FromServices] IFeatureManager manager, string name) =>
{
    if (await manager.IsEnabledAsync("sayHello"))
    {
        return TypedResults.Ok($"hello {name}");
    }
    return TypedResults.NotFound();

}).WithSummary("sayHello");


app.MapGet("/todo", async Task<IResult> ([FromServices] IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("todo"))
    {
        return TypedResults.Ok($"todo is enabled !");
    }
    return TypedResults.NotFound();

}).WithSummary("todo");


app.MapGet("/featureAlwaysOn", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureAlwaysOn"))
    {
        return TypedResults.Ok($"featureAlwaysOn is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("AlwaysOn 过滤器测试");

app.MapGet("/featureTimeWindow", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureTimeWindow"))
    {
        return TypedResults.Ok($"featureTimeWindow is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("TimeWindow 过滤器测试");

app.MapGet("/featurePercentage", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featurePercentage"))
    {
        return TypedResults.Ok($"featurePercentage is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("Percentage 过滤器测试");

app.MapGet("/featureRequirementTypeAll", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureRequirementTypeAll"))
    {
        return TypedResults.Ok($"featureRequirementTypeAll is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("RequirementTypeAll 多过滤器测试");

app.MapGet("/featureAuthencatedGroup", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureAuthencatedGroup"))
    {
        return TypedResults.Ok($"featureAuthencatedGroup is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("AuthencatedGroup 自定义过滤器测试");

//最小Api分组功能添加
{
    var userGroup = app.MapGroup("User").WithTags("User").AddEndpointFilter<UserApiFeatureFilter>(); ;

    userGroup.MapGet("/featureUserApi", IResult (IFeatureManager manager) =>
    {
        return TypedResults.Ok($"featureUserApi is enabled !");

    }).WithSummary("featureUserApi 最小Api过滤器测试");
}

app.Run();

