using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Terraria;

namespace AquaRegia.Weapons.Space;

public class SpaceLaser : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public LaserModule Laser { get; private set; }

    public SpaceLaser() : base()
    {
        Laser = new LaserModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        _immunity.ImmunityTime = 10;

        Laser.SetDefaults(this);
        Laser.SetTexture(TexturesPath.Weapons + "Space/SpaceLaser");

        Projectile.damage = 1;
        Projectile.knockBack = 1;
        Projectile.timeLeft = 2;
    }

    public override bool? Colliding(Microsoft.Xna.Framework.Rectangle projHitbox,
                                    Microsoft.Xna.Framework.Rectangle targetHitbox)
    {
        return Laser.Colliding(projHitbox, targetHitbox);
    }

    public override void AI()
    {
        base.AI();

        var player = Main.LocalPlayer;
        var direction = Main.MouseWorld - player.Center;
        Laser.Update(player.Center, direction);

        Projectile.timeLeft = 2;
        Projectile.Center = player.Center;
    }

    public override bool PreDraw(ref Microsoft.Xna.Framework.Color lightColor)
    {
        Laser.DrawLaser();

        return false;
    }
}
