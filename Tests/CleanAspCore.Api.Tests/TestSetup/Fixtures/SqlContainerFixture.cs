using CleanAspCore.TestUtils.DataBaseSetup;
using Testcontainers.MsSql;

[assembly: AssemblyFixture(typeof(SqlContainerFixture))]

namespace CleanAspCore.Api.Tests.TestSetup.Fixtures;

public sealed class SqlContainerFixture : IAsyncDisposable
{
    public MsSqlContainer Container { get; }

    public SqlContainerFixture()
    {
        var container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04")
            .Build();
        container.StartAsync().RunSynchronouslyWithoutSynchronizationContext();
        Container = container;
    }

    public async ValueTask DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
