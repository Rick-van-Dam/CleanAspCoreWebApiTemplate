using CleanAspCore.Core.Data;

namespace CleanAspCore.Api.Endpoints.Employees;

internal static class DeleteEmployeeById
{
    internal static async Task<Results<NoContent, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var result = await context.Employees
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result switch
        {
            1 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };
    }
}
