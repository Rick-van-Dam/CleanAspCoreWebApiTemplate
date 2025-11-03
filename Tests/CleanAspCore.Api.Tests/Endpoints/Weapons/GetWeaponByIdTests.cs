using CleanAspCore.Api.Endpoints.Weapons;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

public sealed class GetWeaponByIdTests(TestWebApiFixture testWebApiFixture) : ApiTestBase(testWebApiFixture)
{
    [Fact]
    public async Task GetWeaponById_Sword_ReturnsExpectedWeapon()
    {
        //Arrange
        var weapon = new SwordFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Weapons.Add(weapon);
        });

        //Act
        var response = await Sut.CreateUntypedClientFor().GetFromJsonAsync<IWeaponResponse>($"weapons/{weapon.Id}", cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        response.Should().BeOfType<GetSwordResponse>().Which.Id.Should().Be(weapon.Id);
    }

    [Fact]
    public async Task GetWeaponById_Bow_ReturnsExpectedWeapon()
    {
        //Arrange
        var weapon = new BowFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Weapons.Add(weapon);
        });

        //Act
        var response = await Sut.CreateUntypedClientFor().GetFromJsonAsync<IWeaponResponse>($"weapons/{weapon.Id}", cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        response.Should().BeOfType<GetBowResponse>().Which.Id.Should().Be(weapon.Id);
    }
}
