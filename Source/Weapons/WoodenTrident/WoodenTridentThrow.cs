using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTridentThrow : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.WoodenTridentProjectile;

    public bool IsRecalled = false;
    public bool IsCollided = false;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;
        Projectile.scale = 1.2f;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.timeLeft = 10;

        DrawOriginOffsetX = -26;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Vector2.Zero;
        IsCollided = true;

        return false;
    }

    public override bool PreAI()
    {
        Projectile.timeLeft = 10;

        if (IsRecalled)
        {
            Projectile.tileCollide = false;
            var velocity = (Owner.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 36f;
            Projectile.velocity = Projectile.velocity.MoveTowards(velocity, 1f);

            if (Owner.Center.DistanceSQ(Projectile.Center) < 32f * 32f)
            {
                Projectile.Kill();
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-45f);

            return false;
        }

        return base.PreAI();
    }

    public override void AI()
    {
        if (!IsCollided)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            Projectile.velocity.Y += 0.2f;
        }
    }
}