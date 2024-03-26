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
//.NET ���ܹ�������ͨ����ܵı�������ϵͳ���õ�
//Ĭ������£����ܹ������� .NET �������ݵ� "FeatureManagement" �ڼ������ܱ�־����
services.AddFeatureManagement()
    .AddFeatureFilter<AuthenticatedGroupFilter>();

//service.AddFeatureManagement(builder.Configuration.GetSection("CustomFeatureManagement"));
//.AddFeatureFilter<PercentageFilter>();

//���ܹ���ע��Scoped������
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

//�����м���Ĺ��ܿ���
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
}).WithSummary("AlwaysOn ����������");

app.MapGet("/featureTimeWindow", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureTimeWindow"))
    {
        return TypedResults.Ok($"featureTimeWindow is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("TimeWindow ����������");

app.MapGet("/featurePercentage", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featurePercentage"))
    {
        return TypedResults.Ok($"featurePercentage is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("Percentage ����������");

app.MapGet("/featureRequirementTypeAll", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureRequirementTypeAll"))
    {
        return TypedResults.Ok($"featureRequirementTypeAll is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("RequirementTypeAll �����������");

app.MapGet("/featureAuthencatedGroup", async Task<IResult> (IFeatureManager manager) =>
{
    if (await manager.IsEnabledAsync("featureAuthencatedGroup"))
    {
        return TypedResults.Ok($"featureAuthencatedGroup is enabled !");
    }
    return TypedResults.NotFound();
}).WithSummary("AuthencatedGroup �Զ������������");

//��СApi���鹦�����
{
    var userGroup = app.MapGroup("User").WithTags("User").AddEndpointFilter<UserApiFeatureFilter>(); ;

    userGroup.MapGet("/featureUserApi", IResult (IFeatureManager manager) =>
    {
        return TypedResults.Ok($"featureUserApi is enabled !");

    }).WithSummary("featureUserApi ��СApi����������");
}

app.Run();

