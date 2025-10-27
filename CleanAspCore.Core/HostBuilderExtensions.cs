using CleanAspCore.Core.Common.Identity;
using CleanAspCore.Core.Common.Telemetry;
using CleanAspCore.Core.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Core;

public static class HostBuilderExtensions
{
    public static void AddStandardAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.AddServiceDefaults();
        builder.Services.AddMemoryCache();
        builder.AddDataServices();
        builder.AddIdentityServices();
    }
}
