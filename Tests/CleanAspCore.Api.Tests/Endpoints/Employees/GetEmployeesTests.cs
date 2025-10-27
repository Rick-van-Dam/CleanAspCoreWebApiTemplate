using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class GetEmployees(TestWebApi sut)
{
    [Test]
    public async Task? GetEmployees_NoEmployees_ReturnsEmptyPage()
    {
        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadRole).GetEmployees(1, 10);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.OK);
        await Assert.That(response).HasJsonBodyEquivalentTo(new
        {
            TotalPages = 0,
            TotalRecords = 0,
            PageNumber = 0,
            Data = new List<Guid>()
        });
    }

    [Test]
    public async Task GetEmployees_FirstPage_ReturnsExpectedEmployees()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        var job = new JobFaker().Generate();
        var employees = new EmployeeFaker()
            .RuleFor(x => x.Department, department)
            .RuleFor(x => x.Job, job)
            .Generate(15);
        sut.SeedData(context =>
        {
            context.Employees.AddRange(employees);
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadRole).GetEmployees(1, 10);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.OK);
        await Assert.That(response).HasJsonBodyEquivalentTo(new
        {
            TotalPages = 2,
            TotalRecords = 15,
            PageNumber = 1,
            Data = employees
                .OrderBy(x => x.FirstName)
                .Select(x => new { x.Id })
                .Take(10)
                .ToList()
        });
    }

    [Test]
    public async Task GetEmployees_SecondPage_ReturnsExpectedEmployees()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        var job = new JobFaker().Generate();
        var employees = new EmployeeFaker()
            .RuleFor(x => x.Department, department)
            .RuleFor(x => x.Job, job)
            .Generate(15);
        sut.SeedData(context =>
        {
            context.Employees.AddRange(employees);
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadRole).GetEmployees(2, 10);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.OK);
        await Assert.That(response).HasJsonBodyEquivalentTo(new
        {
            TotalPages = 2,
            TotalRecords = 15,
            PageNumber = 2,
            Data = employees
                .OrderBy(x => x.FirstName)
                .Select(x => new { x.Id })
                .Skip(10)
                .Take(10)
                .ToList()
        });
    }
}
