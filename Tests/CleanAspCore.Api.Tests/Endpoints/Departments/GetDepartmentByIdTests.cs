using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

internal sealed class GetDepartmentByIdTests(TestWebApi sut)
{
    [Test]
    public async Task GetDepartmentById_ReturnsExpectedDepartment()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        sut.SeedData(context =>
        {
            context.Departments.Add(department);
        });

        //Act
        var response = await sut.CreateClientFor<IDepartmentApiClient>().GetDepartmentById(department.Id);


        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.OK);
        await Assert.That(response).HasJsonBodyEquivalentTo(new { Id = department.Id });
    }
}
