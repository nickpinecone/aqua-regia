using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTridentThrow : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.wooden_trident_projectile;

    private PropertyModule Property { get; } = new();
    private SpearModule Spear { get; } = new();
    private GravityModule Gravity { get; } = new();

    [RuntimeModule] private ImmunityModule Immunity { get; } = new();
    [RuntimeModule] private RecallModule Recall { get; } = new();
    [RuntimeModule] private AttachModule Attach { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Attach.OnAttach = OnAttach;

        Immunity.SetDefaults();
        Gravity.SetDefaults(0.005f, 0.005f);
        Recall.SetDefaults(36f);

        Property.Set(this)
            .Defaults.TridentThrow()
            .Size(18, 18)
            .TileCollide(false);
    }

    private void OnAttach()
    {
        Gravity.Reset();
    }

    public override void AI()
    {
        base.AI();

        if (!Attach.Attached)
        {
            Spear.ApplyRotation(Projectile);
            Gravity.ApplyGravity(Projectile);
        }
    }
}