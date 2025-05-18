using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Wooden;

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
        Composite.AddRuntimeModule(ImmunityModule.New());

        Property = new PropertyModule();
        Sprite = new SpriteModule();
        HeadBounce = new HeadBounceModule();

        BonkSound = new SoundStyle(AudioPath.Impact + "Bonk")
        {
            Volume = 0.4f,
            PitchVariance = 0.1f,
        };

        Appear = new Animation<int>(10);

        Composite.AddModule(Property, Sprite, HeadBounce);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 20, 20, 1, 5, 0f, 255);
        Property.SetTimeLeft(this, 120);
        Property.SetGravity();
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

        Projectile.velocity = HeadBounce.BounceOff(target, Projectile.Center);
        Property.Gravity /= 1.2f;
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.1f);
    }
}
