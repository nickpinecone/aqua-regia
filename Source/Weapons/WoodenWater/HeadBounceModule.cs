using System;
using AquaRegia.Library;
using AquaRegia.Library.Extended;
using AquaRegia.Library.Extended.Modules;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace AquaRegia.Weapons.WoodenWater;

public class HeadBounceModule : IModule, IProjectileRuntime
{
    public Action? OnHeadBounce { get; set; }

    private SoundStyle BonkSound { get; } = new(Assets.Audio.Impact.bonk)
    {
        Volume = 0.4f,
        PitchVariance = 0.1f,
    };

    public Vector2 BounceOff()
    {
        OnHeadBounce?.Invoke();
        SoundEngine.PlaySound(BonkSound);

        var side = Main.rand.NextFromList(1, -1);
        var sideVector = new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(30 * side));
        var bounceVelocity = sideVector.RotatedByRandom(MathHelper.ToRadians(15));

        bounceVelocity = bounceVelocity.SafeNormalize(Vector2.Zero);
        bounceVelocity *= Main.rand.NextFloat(6f, 8f);

        return bounceVelocity;
    }

    public bool CanHit(NPC target, Vector2 position)
    {
        return target.Top.Y >= position.Y;
    }

    public void RuntimeOnHitNPC(BaseProjectile baseProjectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        baseProjectile.Projectile.velocity = BounceOff();
    }

    public bool? RuntimeCanHitNPC(BaseProjectile baseProjectile, NPC target)
    {
        if (!CanHit(target, baseProjectile.Projectile.Center))
            return false;

        return null;
    }
}