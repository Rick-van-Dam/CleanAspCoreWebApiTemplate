using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Departments;

namespace CleanAspCore.Api.Endpoints.Departments;

/// <summary>
/// The get department response.
/// </summary>
public sealed class GetDepartmentResponse
{
    /// <summary>
    /// The id of this department.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The name of this department.
    /// </summary>
    /// <example>Engineering</example>
    public required string Name { get; init; }

    /// <summary>
    /// The city which this department is in.
    /// </summary>
    /// <example>Amsterdam</example>
    public required string City { get; init; }
}

internal static class GetDepartmentById
{
    internal static async Task<Results<Ok<GetDepartmentResponse>, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var department = await context.Departments
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (department == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(department);
    }

    private static GetDepartmentResponse ToDto(this Department department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        City = department.City
    };
}
