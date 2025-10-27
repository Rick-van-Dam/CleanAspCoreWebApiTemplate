using CleanAspCore.Api.Tests;
using CleanAspCore.Core.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TUnit.Core;
using TUnit.Core.Interfaces;

[assembly: ClassConstructor<DependencyInjectionClassConstructor>]
[assembly: ExcludeFromCodeCoverage]

namespace CleanAspCore.Api.Tests;

public class DependencyInjectionClassConstructor : IClassConstructor, ITestEndEventReceiver
{
    private static readonly IServiceProvider _serviceProvider = CreateServiceProvider();

    private AsyncServiceScope _scope;

    public object Create(Type type, ClassConstructorMetadata classConstructorMetadata)
    {
        _scope = _serviceProvider.CreateAsyncScope();
        return ActivatorUtilities.GetServiceOrCreateInstance(_scope.ServiceProvider, type);
    }

    public ValueTask OnTestEnd(AfterTestContext afterTestContext)
    {
        return ValueTask.CompletedTask;
    }

    private static ServiceProvider CreateServiceProvider() =>
        new ServiceCollection()
            .AddLogging(x => x.AddConsole())
            .RegisterSqlContainer()
            .RegisterMigrationInitializer<HrContext>()
            .AddScoped<TestWebApi>()
            .BuildServiceProvider();
}
