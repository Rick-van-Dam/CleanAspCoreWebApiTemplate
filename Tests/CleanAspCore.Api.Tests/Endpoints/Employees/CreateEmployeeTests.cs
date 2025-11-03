using CleanAspCore.Api.Endpoints.Employees;
using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.TestUtils;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

public sealed class CreateEmployeeTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task CreateEmployee_IsAdded()
    {
        //Arrange
        var createEmployeeRequest = new CreateEmployeeRequestFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, createEmployeeRequest.DepartmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, createEmployeeRequest.JobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }

    public static TheoryData<TestScenario<(FakerConfigurator<CreateEmployeeRequest> configurator, string[] expectedErrors)>> ValidationTestCases() =>
        new(new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "FirstName is null", (x => x.RuleFor(y => y.FirstName, (string?)null), ["FirstName"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "LastName is null", (x => x.RuleFor(y => y.LastName, (string?)null), ["LastName"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "Gender is null", (x => x.RuleFor(y => y.Gender, (string?)null), ["Gender"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "Email is null", (x => x.RuleFor(y => y.Email, (string?)null), ["Email"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "Invalid email", (x => x.RuleFor(y => y.Email, "this is not a valid email address"), ["Email"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "Job does not exist", (x => x.RuleFor(y => y.JobId, Guid.NewGuid()), ["JobId"])), new TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>(
            "Department does not exist", (x => x.RuleFor(y => y.DepartmentId, Guid.NewGuid()), ["DepartmentId"])));

    [Theory]
    [MemberData(nameof(ValidationTestCases))]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest(TestScenario<(FakerConfigurator<CreateEmployeeRequest> configurator, string[] expectedErrors)> scenario)
    {
        //Arrange
        var departmentId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        var createEmployeeRequest = scenario.Input.configurator(new CreateEmployeeRequestFaker()
            .RuleFor(x => x.DepartmentId, departmentId)
            .RuleFor(x => x.JobId, jobId)).Generate();

        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, departmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, jobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertBadRequest(scenario.Input.expectedErrors);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }
}
