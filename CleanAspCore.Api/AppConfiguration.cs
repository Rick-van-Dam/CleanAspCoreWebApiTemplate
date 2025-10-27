using CleanAspCore.Api.Endpoints.Departments;
using CleanAspCore.Api.Endpoints.Employees;
using CleanAspCore.Api.Endpoints.Jobs;
using CleanAspCore.Api.Endpoints.Weapons;
using CleanAspCore.Core.Data.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace CleanAspCore.Api;

internal static class AppConfiguration
{
    internal static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.AddEmployeeServices();
    }

    internal static void AddAppRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
        host.AddWeaponsRoutes();
    }

    internal static void AddAuthServices(this WebApplicationBuilder builder)
    {
        var defaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(Constants.AzureAd);

        var auth = builder.Services.AddAuthentication(Constants.AzureAd);

        auth.AddMicrosoftIdentityWebApi(builder.Configuration, jwtBearerScheme: Constants.AzureAd);

        if (builder.Environment.IsDevelopment())
        {
            auth.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

            defaultPolicy = defaultPolicy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        }

        builder.Services.AddAuthorizationBuilder()
            .AddFallbackPolicy("Fallback", defaultPolicy.Build());
    }

    internal static void RunMigrations(this WebApplication app)
    {
        if (!app.Configuration.GetValue<bool>("EnableAutoMigrations"))
            return;

        if (app.Environment.IsDevelopment())
        {
            var watchIteration = app.Configuration.GetValue("DOTNET_WATCH_ITERATION", 1);
            if (watchIteration == 1)
            {
                app.EnsureHrContextDatabaseIsCreated();
            }
        }
        else
        {
            app.MigrateHrContextDatabase();
        }
    }
}
