namespace CleanAspCore.Api.Tests.Endpoints;

public class HealthCheckTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task GetHealth_Returns200()
    {
        //Act
        var response = await Sut.CreateUntypedClientFor().GetAsync("/health", TestContext.Current.CancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAlive_Returns200()
    {
        //Act
        var response = await Sut.CreateUntypedClientFor().GetAsync("/alive", TestContext.Current.CancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
    }
}
