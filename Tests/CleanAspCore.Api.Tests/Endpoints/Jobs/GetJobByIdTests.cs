using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

internal sealed class GetJobByIdTests(TestWebApi sut)
{
    [Test]
    public async Task GetJobById_ReturnsExpectedJob()
    {
        //Arrange
        var job = new JobFaker().Generate();
        sut.SeedData(context =>
        {
            context.Jobs.Add(job);
        });

        //Act
        var response = await sut.CreateClientFor<IJobApiClient>().GetJobById(job.Id);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.OK);
        await Assert.That(response).HasJsonBodyEquivalentTo(new { Id = job.Id });
    }
}
