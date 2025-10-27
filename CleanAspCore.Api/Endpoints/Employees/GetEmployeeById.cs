using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Employees;

namespace CleanAspCore.Api.Endpoints.Employees;

/// <summary>
/// The get employee response.
/// </summary>
public sealed class GetEmployeeResponse
{
    /// <summary>
    /// The id of this employee.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The firstname of this employee.
    /// </summary>
    /// <example>Mary</example>
    public required string FirstName { get; init; }

    /// <summary>
    /// The lastname of this employee.
    /// </summary>
    /// <example>Poppins</example>
    public required string LastName { get; init; }

    /// <summary>
    /// The email of this employee.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The gender of this employee.
    /// </summary>
    /// <example>Female</example>
    public required string Gender { get; init; }

    /// <summary>
    /// The department id of which this employee is in.
    /// </summary>
    public required Guid DepartmentId { get; init; }

    /// <summary>
    /// The job id of this employee.
    /// </summary>
    public required Guid JobId { get; init; }
}

internal static class GetEmployeeById
{
    internal static async Task<Results<Ok<GetEmployeeResponse>, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var employee = await context.Employees
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (employee == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(employee);
    }

    private static GetEmployeeResponse ToDto(this Employee employee) => new()
    {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email.ToString(),
        Gender = employee.Gender,
        DepartmentId = employee.DepartmentId,
        JobId = employee.JobId
    };
}
