using System.Reflection;
using CleanAspCore.Api;
using CleanAspCore.Api.Common.ErrorHandling;
using CleanAspCore.Core;
using CleanAspCore.Core.Common.OpenApi;
using CleanAspCore.ServiceDefaults;
using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

builder.AddOpenApiServices<CleanAspCore.Api.Program>(AppJsonSerializerContext.Default);
builder.AddAuthServices();
builder.AddAppServices();
builder.AddServiceDefaults();
builder.AddExceptionHandlers();
builder.Services.AddHttpClient();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
builder.AddStandardAppServices();

builder.Configuration.AddJsonFile("appsettings.Local.json", true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
var app = builder.Build();

app.RunMigrations();

app.UseOpenApi();
app.MapDefaultEndpoints();

app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.AddAppRoutes();

app.Run();

namespace CleanAspCore.Api
{
#pragma warning disable CA1515
    public partial class Program
#pragma warning restore CA1515
    {
    }
}
