using CleanAspCore.Core.Common.Paging;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Employees;

namespace CleanAspCore.Api.Endpoints.Employees;

internal static class GetEmployees
{
    internal static async Task<Ok<PagedList<GetEmployeeResponse>>> Handle(HrContext context, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var result = await context.Employees
            .OrderBy(x => x.FirstName)
            .Select(x => x.ToDto())
            .ToPagedListAsync(page, pageSize, cancellationToken);
        return TypedResults.Ok(result);
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
