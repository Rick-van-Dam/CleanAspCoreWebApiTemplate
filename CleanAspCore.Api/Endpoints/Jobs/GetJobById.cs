using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Jobs;

namespace CleanAspCore.Api.Endpoints.Jobs;

/// <summary>
/// The get job response.
/// </summary>
public sealed class GetJobResponse
{
    /// <summary>
    /// The id of this job.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of this job.
    /// </summary>
    /// <example>Software Engineer</example>
    public required string Name { get; init; }
}

internal static class GetJobById
{
    internal static async Task<Results<Ok<GetJobResponse>, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (job == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(job);
    }

    private static GetJobResponse ToDto(this Job department) => new()
    {
        Id = department.Id,
        Name = department.Name
    };
}
