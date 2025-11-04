namespace CleanAspCore.Api.Tests.Endpoints.Departments;

public sealed class GetDepartmentByIdTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task GetDepartmentById_ReturnsExpectedDepartment()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(department);
        });

        //Act
        var response = await Sut.CreateClientFor<IDepartmentApiClient>().GetDepartmentById(department.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new { Id = department.Id });
    }
}
