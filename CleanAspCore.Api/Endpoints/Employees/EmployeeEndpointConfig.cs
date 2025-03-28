﻿using CleanAspCore.Core.Common.GenericValidation;

namespace CleanAspCore.Api.Endpoints.Employees;

internal static class EmployeeEndpointConfig
{
    private const string ReadEmployeesPolicy = "ReadEmployees";
    private const string WriteEmployeesPolicy = "WriteEmployees";

    internal static void AddEmployeeServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ReadEmployeesPolicy, policy => policy.RequireRole("read"))
            .AddPolicy(WriteEmployeesPolicy, policy => policy.RequireRole("write"));
    }

    internal static void AddEmployeesRoutes(this IEndpointRouteBuilder host)
    {
        var employeeGroup = host
            .MapGroup("/employees")
            .WithTags("Employees");

        employeeGroup.MapPost("/", AddEmployee.Handle)
            .RequireAuthorization(WriteEmployeesPolicy)
            .WithRequestBodyValidation();

        employeeGroup.MapGet("/{id:guid}", GetEmployeeById.Handle)
            .RequireAuthorization(ReadEmployeesPolicy)
            .WithName(nameof(GetEmployeeById));

        employeeGroup.MapGet("/", GetEmployees.Handle)
            .RequireAuthorization(ReadEmployeesPolicy)
            .WithName(nameof(GetEmployees));

        employeeGroup.MapDelete("/{id:guid}", DeleteEmployeeById.Handle)
            .RequireAuthorization(WriteEmployeesPolicy);

        employeeGroup.MapPatch("/{id:guid}", UpdateEmployeeById.Handle)
            .RequireAuthorization(WriteEmployeesPolicy)
            .WithRequestBodyValidation();
    }
}
