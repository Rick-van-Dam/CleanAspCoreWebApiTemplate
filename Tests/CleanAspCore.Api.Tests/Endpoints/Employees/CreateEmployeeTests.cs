using CleanAspCore.Api.Endpoints.Employees;
using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.TestUtils;
using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class CreateEmployeeTests(TestWebApi sut)
{
    [Test]
    public async Task CreateEmployee_IsAdded()
    {
        //Arrange
        var createEmployeeRequest = new CreateEmployeeRequestFaker().Generate();
        sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, createEmployeeRequest.DepartmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, createEmployeeRequest.JobId).Generate());
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Employees)
                .HasCount().EqualTo(1).And
                .Contains(x => x.Id == createdId);
        });
    }

    public static IEnumerable<Func<TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>>> ValidationTestCases()
    {
        yield return () => new((string)"FirstName is null",
            (x => x.RuleFor(y => y.FirstName, (string?)null), ["FirstName"]));
        yield return () => new((string)"LastName is null",
            (x => x.RuleFor(y => y.LastName, (string?)null), ["LastName"]));
        yield return () => new((string)"Gender is null",
            (x => x.RuleFor(y => y.Gender, (string?)null), ["Gender"]));
        yield return () => new((string)"Email is null",
            (x => x.RuleFor(y => y.Email, (string?)null), ["Email"]));
        yield return () => new((string)"Invalid email",
            (x => x.RuleFor(y => y.Email, "this is not a valid email address"), ["Email"]));
        yield return () => new((string)"Job does not exist",
            (x => x.RuleFor(y => y.JobId, Guid.NewGuid()), ["JobId"]));
        yield return () => new((string)"Department does not exist",
            (x => x.RuleFor(y => y.DepartmentId, Guid.NewGuid()), ["DepartmentId"]));
    }

    [Test]
    [MethodDataSource(nameof(ValidationTestCases))]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest(TestScenario<(FakerConfigurator<CreateEmployeeRequest> configurator, string[] expectedErrors)> scenario)
    {
        //Arrange
        var departmentId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        var createEmployeeRequest = scenario.Input.configurator(new CreateEmployeeRequestFaker()
            .RuleFor(x => x.DepartmentId, departmentId)
            .RuleFor(x => x.JobId, jobId)).Generate();

        sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, departmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, jobId).Generate());
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await Assert.That(response).HasBadRequest(scenario.Input.expectedErrors);
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Employees).IsEmpty();
        });
    }
}
