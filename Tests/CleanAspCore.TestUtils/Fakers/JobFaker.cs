using Bogus;
using CleanAspCore.Core.Data.Models.Jobs;

namespace CleanAspCore.TestUtils.Fakers;

public sealed class JobFaker : Faker<Job>
{
    public JobFaker()
    {
        UseSeed(1);
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Name.JobTitle());
    }
}
