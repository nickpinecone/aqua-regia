using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Wooden;

public class AcornProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Wooden/AcornProjectile";

    public PropertyModule Property { get; private set; }
    public SpriteModule Sprite { get; private set; }
    public HeadBounceModule HeadBounce { get; private set; }

    public SoundStyle BonkSound { get; private set; }
    public Animation<int> Appear { get; private set; }

    public AcornProjectile() : base()
    {
        Property = new PropertyModule(this);
        Sprite = new SpriteModule(this);
        HeadBounce = new HeadBounceModule(this, Property);

        BonkSound = new SoundStyle(AudioPath.Impact + "Bonk")
        {
            Volume = 0.4f,
            PitchVariance = 0.1f,
        };

        Appear = new Animation<int>(10);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 120);

        Projectile.damage = 1;
        Projectile.penetrate = 5;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 255;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        var particle = Particle.Single(ParticleID.Wood, Projectile.Center, new Vector2(10, 10), Vector2.Zero);
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

        Projectile.velocity = HeadBounce.BounceOff(target, Projectile.Center) ?? Projectile.velocity;
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.1f);
    }
}
