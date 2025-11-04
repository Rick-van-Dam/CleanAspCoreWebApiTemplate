using CleanAspCore.Core.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: AssemblyFixture(typeof(TestWebApiFixture))]
[assembly: CaptureConsole]

namespace CleanAspCore.Api.Tests.TestSetup.Fixtures;

public sealed class TestWebApiFixture
{
    private readonly IServiceProvider _serviceProvider = CreateServiceProvider();

    private static ServiceProvider CreateServiceProvider() =>
        new ServiceCollection()
            .AddLogging(x => x.AddConsole())
            .RegisterSqlContainer()
            .RegisterMigrationInitializer<HrContext>()
            .AddScoped<TestWebApi>()
            .BuildServiceProvider();

    public AsyncServiceScope CreateScope() => _serviceProvider.CreateAsyncScope();
}
