﻿using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Features.Departments;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class GetDepartmentByIdTests : TestBase
{
    [Test]
    public async Task GetDepartmentById_ReturnsExpectedDepartment()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(department);
        });

        //Act
        var response = await Sut.CreateClientFor<IDepartmentApiClient>().GetDepartmentById(department.Id);


        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new { Id = department.Id });
    }
}
