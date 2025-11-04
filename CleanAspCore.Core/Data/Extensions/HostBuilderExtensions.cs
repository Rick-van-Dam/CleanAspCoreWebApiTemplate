using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanAspCore.Core.Data.Extensions;

public static class HostBuilderExtensions
{
    public static void AddDataServices(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<HrContext>(HrContext.ConnectionStringName);
        builder.Services.AddSingleton<TokenCredentialDbInterceptor>();
        builder.Services.ConfigureDbContext<HrContext>((provider, options) =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            options.UseLoggerFactory(loggerFactory);
            options.AddInterceptors(provider.GetRequiredService<TokenCredentialDbInterceptor>());
        });
    }
}
