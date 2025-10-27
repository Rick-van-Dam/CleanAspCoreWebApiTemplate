using CleanAspCore.Core.Common.NullableValidation;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Jobs;

namespace CleanAspCore.Api.Endpoints.Jobs;

/// <summary>
/// A request to create a new job.
/// </summary>
public sealed class CreateJobRequest
{
    /// <summary>
    /// The name of this job.
    /// </summary>
    /// <example>Software Engineer</example>
    public required string Name { get; init; }
}

internal sealed class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
{
    public CreateJobRequestValidator()
    {
        this.ValidateNullableReferences();
    }
}

internal static class AddJobs
{
    internal static async Task<CreatedAtRoute> Handle([FromBody] CreateJobRequest createJobRequest, HrContext context, CancellationToken cancellationToken)
    {
        var job = createJobRequest.ToJob();

        context.Jobs.AddRange(job);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetJobById), new { job.Id });
    }

    private static Job ToJob(this CreateJobRequest createJobRequest) => new()
    {
        Id = Guid.NewGuid(),
        Name = createJobRequest.Name
    };
}
