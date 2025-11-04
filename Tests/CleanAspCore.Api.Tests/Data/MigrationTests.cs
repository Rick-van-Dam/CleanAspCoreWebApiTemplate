using CleanAspCore.Core.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using CleanAspCore.TestUtils.Logging;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CleanAspCore.Api.Tests.Data;

public sealed class MigrationTests(TestWebApiFixture testWebApiFixture, ITestOutputHelper output)
{
    public static TheoryData<MigrationScript> MigrationTestCases()
    {
        using DbContext context = new HrContext();
        return new(context.GenerateMigrationScripts());
    }

    [Theory]
    [MemberData(nameof(MigrationTestCases))]
    public async Task MigrationsUpAndDown_NoErrors(MigrationScript migration)
    {
        await using var scope = testWebApiFixture.CreateScope();
        var container = scope.ServiceProvider.GetRequiredService<MsSqlContainer>();
        var databaseName = "MigrationsTest";
        await container.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(container, new XunitLogger("MigrationTests", output), databaseName);
        var upResult = await migrator.Up(migration);
        AssertMigrationResult(upResult);
        var downResult = await migrator.Down(migration);
        AssertMigrationResult(downResult);
        var upResult2 = await migrator.Up(migration);
        AssertMigrationResult(upResult2);
    }

    private static void AssertMigrationResult(ExecResult result)
    {
        using var s = new AssertionScope();
        result.ExitCode.Should().Be(0);
        s.AppendTracing(result.Stderr);
    }

    [Fact]
    public async Task ModelShouldNotHavePendingModelChanges()
    {
        await using DbContext context = new HrContext();
        var hasPendingModelChanges = context.Database.HasPendingModelChanges();
        Assert.False(hasPendingModelChanges);
    }
}
