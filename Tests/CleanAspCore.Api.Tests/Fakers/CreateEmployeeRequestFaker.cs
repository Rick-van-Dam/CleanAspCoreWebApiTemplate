using Bogus;
using CleanAspCore.Api.Endpoints.Employees;
using CleanAspCore.Core.Common.ValueObjects;

namespace CleanAspCore.Api.Tests.Fakers;

public sealed class CreateEmployeeRequestFaker : Faker<CreateEmployeeRequest>
{
    public CreateEmployeeRequestFaker()
    {
        UseSeed(3);
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
        RuleFor(x => x.Email, f => new EmailAddress(f.Internet.Email()));
        RuleFor(x => x.Gender, f => f.PickRandom("Male", "Female"));
        RuleFor(x => x.DepartmentId, f => f.Random.Guid());
        RuleFor(x => x.JobId, f => f.Random.Guid());
    }
}
