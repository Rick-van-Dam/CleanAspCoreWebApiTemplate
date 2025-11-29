using CleanAspCore.Core.Common.NullableValidation;
using CleanAspCore.Core.Common.SetProperty;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Employees;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;

namespace CleanAspCore.Api.Endpoints.Employees;

/// <summary>
/// A request to update a employee.
/// </summary>
public sealed class UpdateEmployeeRequest
{
    /// <summary>
    /// The firstname of this employee.
    /// </summary>
    /// <example>Mary</example>
    public string? FirstName { get; init; }

    /// <summary>
    /// The lastname of this employee.
    /// </summary>
    /// <example>Poppins</example>
    public string? LastName { get; init; }

    /// <summary>
    /// The email of this employee.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The gender of this employee.
    /// </summary>
    /// <example>Female</example>
    public string? Gender { get; init; }

    /// <summary>
    /// The department id of which this employee is in.
    /// </summary>
    public Guid? DepartmentId { get; init; }

    /// <summary>
    /// The job id of this employee.
    /// </summary>
    public Guid? JobId { get; init; }
}

internal sealed class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        this.ValidateNullableReferences();

        RuleFor(x => x.Email).EmailAddress();
    }
}

internal static class UpdateEmployeeById
{
    internal static async Task<Results<NoContent, NotFound>> Handle(
        Guid id, [FromBody] UpdateEmployeeRequest updateEmployeeRequest, HrContext context, CancellationToken cancellationToken)
    {
        var changed = await context.Employees
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetPropertyIfNotNull(x => x.FirstName, updateEmployeeRequest.FirstName)
                .SetPropertyIfNotNull(x => x.LastName, updateEmployeeRequest.LastName)
                .SetPropertyIfNotNull(x => x.Email, updateEmployeeRequest.Email)
                .SetPropertyIfNotNull(x => x.Gender, updateEmployeeRequest.Gender)
                .SetPropertyIfNotNull(x => x.DepartmentId, updateEmployeeRequest.DepartmentId)
                .SetPropertyIfNotNull(x => x.JobId, updateEmployeeRequest.JobId), cancellationToken);

        return changed switch
        {
            1 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };
    }
}
