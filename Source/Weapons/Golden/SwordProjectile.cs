using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Players;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Golden;

public class SwordProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Golden/SwordProjectile";

    public Animation<int> Appear { get; private set; }
    public PropertyModule Property { get; private set; }
    public StickModule Stick { get; private set; }

    public Vector2 Size { get; private set; }
    public Rectangle WorldRectangle { get; private set; }

    public SoundStyle MetalHitSound { get; private set; }
    public Vector2 InitialVelocity { get; set; } = Vector2.Zero;

    private Vector2 _localShift = Vector2.Zero;
    private Vector2 _beforeHit = Vector2.Zero;

    public SwordProjectile() : base()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();
        Stick = new StickModule();

        Composite.AddModule(Property, Stick);

        Appear = new Animation<int>(10);

        MetalHitSound = new SoundStyle(AudioPath.Impact + "MetalHit")
        {
            Volume = 0.7f,
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 40, 40, 1, -1, 0, 255, 100, false);
        Property.SetTimeLeft(this, 80);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Size = new Vector2(Projectile.width * 0.4f, Projectile.height * 0.4f);
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (Stick.Target == null && !target.friendly && target.getRect().Intersects(Projectile.getRect()))
        {
            SoundEngine.PlaySound(MetalHitSound);
            Projectile.timeLeft = 360;
            _beforeHit = Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
        }

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Projectile.friendly = false;
        Owner.GetModPlayer<ScreenShake>().Activate(6, 4);

        var invert = _beforeHit.RotatedBy(MathHelper.Pi);
        var start = invert.RotatedBy(-MathHelper.PiOver4);
        var end = invert.RotatedBy(MathHelper.PiOver4);

        var offset = _beforeHit.SafeNormalize(Vector2.Zero);
        Particle.Arc(DustID.Blood, (Vector2)Stick.HitPoint! + offset * (Projectile.height / 2), new Vector2(6, 6),
                     start, end, 6, 3f, 1.4f);
    }

    public void Push()
    {
        if (Stick.Target != null)
        {
            var shift = Vector2.UnitX.RotatedBy(_beforeHit.ToRotation()) * 6f;
            var rect = new Rectangle((int)(Projectile.position.X + shift.X), (int)(Projectile.position.Y + shift.Y),
                                     Projectile.width, Projectile.height);

            if (rect.Intersects(Stick.Target.getRect()))
            {
                SoundEngine.PlaySound(MetalHitSound);

                _localShift += shift;
                Projectile.damage = (int)(Projectile.damage * 1.1f);
                Projectile.friendly = true;
            }
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Owner.GetModPlayer<GoldenPlayer>().RemoveSword(this);

        if (timeLeft > 0)
        {
            var particle = Particle.Single(DustID.Platinum, Projectile.Center, new Vector2(10, 10), Vector2.Zero, 1.2f);
            particle.noGravity = true;
        }
    }

    public override void AI()
    {
        base.AI();

        if (Projectile.timeLeft <= 10)
        {
            Projectile.alpha = Appear.Backwards() ?? Projectile.alpha;
        }

        Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;

        if (Appear.Finished)
        {
            if (Stick.Target == null)
            {
                Projectile.velocity = InitialVelocity;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else if (Stick.Target != null)
            {
                var direction = Vector2.UnitX.RotatedBy(_beforeHit.ToRotation()).RotatedBy(MathHelper.Pi);
                var handle = Projectile.Center + direction * Projectile.height;
                handle = handle - (new Vector2(1, 1) * (Size.X / 2));
                WorldRectangle = new Rectangle((int)(handle.X), (int)(handle.Y), (int)Size.X, (int)Size.Y);

                if (Stick.Update() == null)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.Center = (Vector2)Stick.HitPoint! + _localShift;
                }
            }
        }
    }
}
