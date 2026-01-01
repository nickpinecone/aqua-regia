using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTridentThrow : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.WoodenTridentProjectile;

    public bool IsCollided = false;

    private PropertyModule Property { get; } = new();
    private GravityModule Gravity { get; } = new();

    [RuntimeModule] private RecallModule Recall { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Gravity.SetDefaults();
        Recall.SetDefaults(36f);

        Property.Set(this)
            .Size(18, 18, 1.2f)
            .Friendly(true, false)
            .Damage(DamageClass.Melee, -1)
            .TileCollide(true)
            .TimeLeft(10)
            .DrawOffset(0, drawOriginOffset: new Vector2(-26, 0));
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        Projectile.velocity = Vector2.Zero;
        IsCollided = true;

        return false;
    }

    public override void AI()
    {
        base.AI();

        if (!IsCollided)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            Gravity.RuntimeAI(this);
        }
    }
}