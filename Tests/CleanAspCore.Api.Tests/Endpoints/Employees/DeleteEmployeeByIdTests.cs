namespace CleanAspCore.Api.Tests.Endpoints.Employees;

public sealed class DeleteEmployeeByIdTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task DeleteEmployeeById_IsDeleted()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).DeleteEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NoContent);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task DeleteEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).DeleteEmployeeById(id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NotFound);
    }
}
