using Microsoft.Extensions.DependencyInjection;

namespace CleanAspCore.Api.Tests.TestSetup.Fixtures;

public abstract class ApiTestBase : IAsyncDisposable
{
    protected TestWebApi Sut { get; }

    private readonly AsyncServiceScope _scope;

    protected ApiTestBase(TestWebApiFixture testWebApiFixture)
    {
        _scope = testWebApiFixture.CreateScope();
        Sut = _scope.ServiceProvider.GetRequiredService<TestWebApi>();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _scope.DisposeAsync();
    }
}
