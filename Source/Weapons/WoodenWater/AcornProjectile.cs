using AquaRegia.Library;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using AquaRegia.Library.Tween;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenWater;

public class AcornProjectile : BaseProjectile
{
    public override string Texture => Assets.Weapons + $"{nameof(WoodenWater)}/{nameof(AcornProjectile)}";

    private PropertyModule Property { get; }
    private RotateOnMoveModule RotateOnMove { get; }
    private GravityModule Gravity { get; }
    private HeadBounceModule HeadBounce { get; }

    private SoundStyle BonkSound { get; }
    private Tween<int> Appear { get; }

    public AcornProjectile()
    {
        Property = new PropertyModule(this);
        RotateOnMove = new RotateOnMoveModule();
        Gravity = new GravityModule();
        HeadBounce = new HeadBounceModule();

        Composite.AddModule(Property, RotateOnMove, Gravity, HeadBounce);
        Composite.AddRuntimeModule(new ImmunityModule(), RotateOnMove, Gravity);

        BonkSound = new SoundStyle(Assets.Audio.Impact + "Bonk")
        {
            Volume = 0.4f,
            PitchVariance = 0.1f,
        };

        Appear = Tween.Create<int>(10);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.Size(20, 20)
            .Damage(DamageClass.Ranged, 5)
            .Friendly(true, false)
            .Alpha(255)
            .TimeLeft(120);

        RotateOnMove.SetAmount();
        Gravity.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var particle = DustHelper.Single(DustExID.Wood, Projectile.Center, new Vector2(10, 10), Vector2.Zero);
        particle.noGravity = true;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (!HeadBounce.CanHit(target, Projectile.Center))
            return false;

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        SoundEngine.PlaySound(BonkSound);

        Projectile.velocity = HeadBounce.BounceOff(target, Projectile.Center);
        Gravity.Value /= 1.2f;
    }

    public override void AI()
    {
        base.AI();

        Appear.Transition(255, 0).OnTransition((value) => { Projectile.alpha = value; });
    }
}