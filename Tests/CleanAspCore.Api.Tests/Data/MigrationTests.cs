using CleanAspCore.Core.Data;
using CleanAspCore.TestUtils.DataBaseSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace CleanAspCore.Api.Tests.Data;

internal sealed class MigrationTests(MsSqlContainer databaseContainer, ILogger<MigrationTests> logger)
{
    public static IEnumerable<Func<MigrationScript>> MigrationTestCases()
    {
        using DbContext context = new HrContext();
        var migrations = context.GenerateMigrationScripts();

        foreach (var migration in migrations)
        {
            yield return () => migration;
        }
    }

    [Test]
    [MethodDataSource(nameof(MigrationTestCases))]
    [NotInParallel("MigrationsTest")]
    public async Task MigrationsUpAndDown_NoErrors(MigrationScript migration)
    {
        var databaseName = "MigrationsTest";
        await databaseContainer.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(databaseContainer, logger, databaseName);
        var upResult = await migrator.Up(migration);
        await Assert.That(upResult.ExitCode).IsEqualTo(0).Because($"Error during migration up: {upResult.Stderr}");
        var downResult = await migrator.Down(migration);
        await Assert.That(downResult.ExitCode).IsEqualTo(0).Because($"Error during migration down: {downResult.Stderr}");
        var upResult2 = await migrator.Up(migration);
        await Assert.That(upResult2.ExitCode).IsEqualTo(0).Because($"Error during migration up2: {upResult2.Stderr}");
    }

    [Test]
    public async Task ModelShouldNotHavePendingModelChanges()
    {
        await using DbContext context = new HrContext();
        var hasPendingModelChanges = context.Database.HasPendingModelChanges();
        await Assert.That(hasPendingModelChanges).IsFalse();
    }
}
