using CleanAspCore.Core.Common.EntityShouldExistValidation;
using CleanAspCore.Core.Common.NullableValidation;
using CleanAspCore.Core.Common.ValueObjects;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Employees;

namespace CleanAspCore.Api.Endpoints.Employees;

/// <summary>
/// A request to create a new employee.
/// </summary>
public sealed class CreateEmployeeRequest
{
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
    public Guid DepartmentId { get; init; }

    /// <summary>
    /// The job id of this employee.
    /// </summary>
    public Guid JobId { get; init; }
}

internal sealed class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator(HrContext context)
    {
        this.ValidateNullableReferences();

        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.JobId).EntityShouldExist(context.Jobs);
        RuleFor(x => x.DepartmentId).EntityShouldExist(context.Departments);
    }
}

internal static class AddEmployee
{
    internal static async Task<CreatedAtRoute> Handle([FromBody] CreateEmployeeRequest request, HrContext context, [FromServices] IValidator<CreateEmployeeRequest> validator,
        CancellationToken cancellationToken)
    {
        var employee = request.ToEmployee();

        context.Employees.AddRange(employee);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetEmployeeById), new { employee.Id });
    }

    private static Employee ToEmployee(this CreateEmployeeRequest employee) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = new EmailAddress(employee.Email),
        Gender = employee.Gender,
        DepartmentId = employee.DepartmentId,
        JobId = employee.JobId
    };
}
