using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace AquaRegia.Weapons.Space;

public class SpaceLaser : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public LaserModule Laser { get; private set; }
    public SoundStyle LaserShot { get; private set; }
    public ImmunityModule Immunity { get; private set; }

    public SpaceLaser() : base()
    {
        Laser = new LaserModule();
        Immunity = new ImmunityModule();

        Composite.AddModule(Laser, Immunity);

        LaserShot = new SoundStyle(AudioPath.Spawn + "LaserShot") with
        {
            PitchVariance = 0.1f,
            Volume = 0.6f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Immunity.SetDefaults(10);

        Laser.SetDefaults(this);
        Laser.SetTexture(TexturesPath.Weapons + "Space/SpaceLaser");

        Projectile.damage = 1;
        Projectile.knockBack = 0;
        Projectile.timeLeft = 2;
    }

    public override bool? Colliding(Microsoft.Xna.Framework.Rectangle projHitbox,
                                    Microsoft.Xna.Framework.Rectangle targetHitbox)
    {
        return Laser.Colliding(projHitbox, targetHitbox);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Immunity.Reset(target);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        SoundEngine.PlaySound(LaserShot);
    }

    public override void AI()
    {
        base.AI();
        Immunity.Update();

        Laser.Update(Projectile.Center, Vector2.UnitY);

        Projectile.timeLeft = 2;
    }

    public override bool PreDraw(ref Microsoft.Xna.Framework.Color lightColor)
    {
        Laser.DrawLaser();

        return false;
    }
}
