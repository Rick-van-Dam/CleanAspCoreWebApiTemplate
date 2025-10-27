using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Core.Data.Extensions;

public static class HrContextExtensions
{
    public static void EnsureHrContextDatabaseIsCreated(this IHost host)
    {
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.EnsureCreated();
    }

    public static void MigrateHrContextDatabase(this IHost host)
    {
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.Migrate();
    }
}
