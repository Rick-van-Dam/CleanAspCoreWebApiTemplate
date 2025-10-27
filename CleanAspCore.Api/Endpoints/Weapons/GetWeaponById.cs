using System.Text.Json.Serialization;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Models.Weapons;

namespace CleanAspCore.Api.Endpoints.Weapons;

/// <summary>
/// Just here to support the derived types serialization.
/// </summary>
[JsonDerivedType(typeof(GetBowResponse), Weapon.Types.Bow)]
[JsonDerivedType(typeof(GetSwordResponse), Weapon.Types.Sword)]
public interface IWeaponResponse
{
    /// <summary>
    /// The id of this weapon.
    /// </summary>
    Guid Id { get; init; }

    /// <summary>
    /// The type of the weapon
    /// </summary>
    string Type { get; init; }
}

/// <summary>
/// The get bow response.
/// </summary>
public sealed class GetBowResponse : IWeaponResponse
{
    /// <inheritdoc />
    public required Guid Id { get; init; }

    /// <inheritdoc />
    public required string Type { get; init; }

    /// <summary>
    /// The range of this bow.
    /// </summary>
    public required float Range { get; init; }

    /// <summary>
    /// The damage the bow does per shot
    /// </summary>
    public required float Damage { get; init; }

    /// <summary>
    /// Amount of shots per second.
    /// </summary>
    public required float RateOfFire { get; init; }
}

/// <summary>
/// The get sword response.
/// </summary>
public sealed class GetSwordResponse : IWeaponResponse
{
    /// <summary>
    /// The id of this weapon.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The type of the weapon
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// The damage the sword does per attack.
    /// </summary>
    public required float Damage { get; init; }

    /// <summary>
    /// Amount of attacks per second.
    /// </summary>
    public required float RateOfFire { get; init; }
}

internal static class GetWeaponById
{
    internal static async Task<Results<Ok<IWeaponResponse>, NotFound>> Handle(Guid id, HrContext context, CancellationToken cancellationToken)
    {
        var weapon = await context.Weapons
            .Where(x => x.Id == id)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (weapon == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(weapon);
    }

    private static IWeaponResponse ToDto(this Weapon weapon) => weapon switch
    {
        Bow bow => new GetBowResponse
        {
            Id = bow.Id,
            Damage = bow.Damage,
            Range = bow.Range,
            RateOfFire = bow.RateOfFire,
            Type = bow.Type
        },
        Sword sword => new GetSwordResponse
        {
            Id = sword.Id,
            Damage = sword.Damage,
            RateOfFire = sword.RateOfFire,
            Type = sword.Type
        },
        _ => throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null)
    };
}
