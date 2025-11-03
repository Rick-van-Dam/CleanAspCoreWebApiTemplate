using CleanAspCore.Api.Endpoints.Weapons;
using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Core.Data.Models.Weapons;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

public sealed class CreateWeaponTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task Create_Sword_IsAdded()
    {
        //Arrange
        var request = new CreateSwordRequestFaker().Generate();

        //Act
        var response = await Sut.CreateUntypedClientFor().PostAsJsonAsync<ICreateWeaponRequest>("/weapons", request, cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Weapons.Should().BeEquivalentTo([new { Id = createdId }]).And.AllBeOfType<Sword>();
        });
    }

    [Fact]
    public async Task Create_Bow_IsAdded()
    {
        //Arrange
        var request = new CreateBowRequestFaker().Generate();

        //Act
        var response = await Sut.CreateUntypedClientFor().PostAsJsonAsync<ICreateWeaponRequest>("/weapons", request, cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Weapons.Should().BeEquivalentTo([new { Id = createdId }]).And.AllBeOfType<Bow>();
        });
    }
}
