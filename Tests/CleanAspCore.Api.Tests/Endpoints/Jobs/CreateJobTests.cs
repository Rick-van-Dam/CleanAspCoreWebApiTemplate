using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

public sealed class CreateJobTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task CreateJob_IsAdded()
    {
        //Arrange
        var createJobRequest = new CreateJobRequestFaker().Generate();

        //Act
        var response = await Sut.CreateClientFor<IJobApiClient>().CreateJob(createJobRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Jobs.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }
}
