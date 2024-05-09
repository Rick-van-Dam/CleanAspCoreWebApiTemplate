﻿using CleanAspCore.Data;
using CleanAspCore.Data.Models;
using CleanAspCore.Extensions.FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanAspCore.Features.Departments.Endpoints;

public sealed class CreateDepartmentRequest
{
    public required string Name { get; init; }
    public required string City { get; init; }
}

public sealed class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        this.ValidateNullableReferences();
    }
}

internal static class AddDepartments
{
    public static async Task<CreatedAtRoute> Handle(HrContext context, CreateDepartmentRequest createDepartmentRequest, CancellationToken cancellationToken)
    {
        var department = createDepartmentRequest.ToDepartment();

        context.Departments.AddRange(department);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetDepartmentById), new { department.Id });
    }

    private static Department ToDepartment(this CreateDepartmentRequest department) => new()
    {
        Id = Guid.NewGuid(),
        Name = department.Name,
        City = department.City
    };
}
