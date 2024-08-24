using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Golden;

public class DaggerProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Golden/DaggerProjectile";

    public Animation<int> Appear { get; private set; }
    public PropertyModule Property { get; private set; }
    public StickModule Stick { get; private set; }

    public SoundStyle SlashSound { get; private set; }
    public Vector2 InitialVelocity { get; set; } = Vector2.Zero;

    private int _penetrateAmount = 0;
    private int _penetrateMax = 3;

    public DaggerProjectile() : base()
    {
        Appear = new Animation<int>(10);
        Property = new PropertyModule(this);
        Stick = new StickModule(this);

        SlashSound = new SoundStyle(AudioPath.Impact + "Slash")
        {
            Volume = 0.5f,
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 60);

        Projectile.tileCollide = false;
        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 10;
        Projectile.height = 24;
        Projectile.alpha = 255;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (Stick.Target == null && !target.friendly && target.getRect().Intersects(Projectile.getRect()))
        {
            SoundEngine.PlaySound(SlashSound);

            Stick.BeforeVelocity = Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
        }

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
        if (_penetrateAmount < _penetrateMax)
        {
            Projectile.timeLeft = Property.DefaultTime;
            _penetrateAmount += 1;

            var invert = Stick.BeforeVelocity.RotatedBy(MathHelper.Pi);
            var start = invert.RotatedBy(-MathHelper.PiOver4);
            var end = invert.RotatedBy(MathHelper.PiOver4);

            var offset = Stick.BeforeVelocity.SafeNormalize(Vector2.Zero);
            Particle.Arc(DustID.Blood, Stick.HitPoint + offset * (Projectile.height / 2), new Vector2(6, 6), start, end,
                         3, 2f, 1f);
        }
        else
        {
            Projectile.timeLeft = 10;
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (_penetrateAmount < _penetrateMax)
        {
            var particle = Particle.Single(DustID.Iron, Projectile.Center, new Vector2(6, 6), Vector2.Zero, 1f);
            particle.noGravity = true;
        }
    }

    public override void AI()
    {
        base.AI();

        if (_penetrateAmount >= _penetrateMax)
        {
            Projectile.friendly = false;
            Projectile.alpha = Appear.Backwards() ?? Projectile.alpha;
        }
        else
        {
            Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;
            // ChatLog.Message(Appear.Value.ToString());
            // ChatLog.Message(Appear.Finished);
        }

        if (Appear.Finished)
        {
            if (Stick.Target == null)
            {
                Projectile.velocity = InitialVelocity;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else if (Stick.Target != null)
            {
                if (Stick.Update() == null)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.Center = Stick.HitPoint;
                }
            }
        }
    }
}
