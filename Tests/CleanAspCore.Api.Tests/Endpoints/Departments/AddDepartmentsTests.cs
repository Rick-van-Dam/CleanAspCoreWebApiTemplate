using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

public sealed class AddDepartmentsTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task CreateDepartment_IsAdded()
    {
        //Arrange
        var department = new CreateDepartmentRequestFaker().Generate();

        //Act
        var response = await Sut.CreateClientFor<IDepartmentApiClient>().CreateDepartment(department);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);

        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Departments.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }

    [Fact]
    public async Task CreateDepartment_MissingProperties_ReturnsBadRequestWithDetails()
    {
        //Act
        var response = await Sut.CreateUntypedClientFor().PostAsJsonAsync("departments", new { }, cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        await response.AssertBadRequest("name", "city");
    }

    [Fact]
    public async Task CreateDepartment_InvalidJson_ReturnsBadRequestWithDetails()
    {
        //Act
        var response = await Sut.CreateUntypedClientFor().PostAsJsonAsync("departments", "{/}", cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        await response.AssertBadRequest();
        var responseText = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        responseText.Should().Contain("The JSON value could not be converted to");
    }
}
