using CleanAspCore.Core.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;
using Xunit;

namespace CleanAspCore.Api.Tests.Data;

public sealed class MigrationTests(MsSqlContainer databaseContainer, ILogger<MigrationTests> logger)
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
        var databaseName = "MigrationsTest";
        await databaseContainer.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(databaseContainer, logger, databaseName);
        var upResult = await migrator.Up(migration);
        AssertMigrationResult(upResult);
        var downResult = await migrator.Down(migration);
        AssertMigrationResult(downResult);
        var upResult2 = await migrator.Up(migration);
        AssertMigrationResult(upResult2);
    }

    private static void AssertMigrationResult(ExecResult result)
    {
        Assert.True(result.ExitCode == 0, $"Error during migration: {result.Stderr}");
    }

    [Fact]
    public async Task ModelShouldNotHavePendingModelChanges()
    {
        await using DbContext context = new HrContext();
        var hasPendingModelChanges = context.Database.HasPendingModelChanges();
        Assert.False(hasPendingModelChanges);
    }
}
