using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTridentProjectile : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.wooden_trident_projectile;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private ImmunityModule Immunity { get; } = new();
    [RuntimeModule] private SpearModule Spear { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Immunity.SetDefaults();
        Spear.SetDefaults();

        Property.Set(this)
            .Defaults.Spear()
            .Size(18, 18);
    }
}