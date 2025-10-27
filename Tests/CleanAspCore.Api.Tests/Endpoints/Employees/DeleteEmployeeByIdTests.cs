using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class DeleteEmployeeByIdTests(TestWebApi sut)
{
    [Test]
    public async Task DeleteEmployeeById_IsDeleted()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).DeleteEmployeeById(employee.Id);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.NoContent);
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Employees).IsEmpty();
        });
    }

    [Test]
    public async Task DeleteEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).DeleteEmployeeById(id);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.NotFound);
    }
}
