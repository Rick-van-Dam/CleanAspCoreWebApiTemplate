using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Core.Data.Extensions;

public static class HostBuilderExtensions
{
    public static void AddDataServices(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<HrContext>(HrContext.ConnectionStringName);
        builder.Services.AddSingleton<TokenCredentialDbInterceptor>();
        builder.Services.AddDbContext<HrContext>((provider, options) =>
        {
            options.AddInterceptors(provider.GetRequiredService<TokenCredentialDbInterceptor>());
        });
    }
}
