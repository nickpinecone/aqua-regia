using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Golden;

public class SwordProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "SwordProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }
    public StickModule Stick { get; private set; }

    public Vector2 Size { get; private set; }
    public Rectangle WorldRectangle { get; private set; }

    public SoundStyle MetalHitSound { get; private set; }
    public Vector2 InitialVelocity { get; set; } = Vector2.Zero;

    private Vector2 _localShift = Vector2.Zero;

    public SwordProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
        Stick = new StickModule(this);

        MetalHitSound = new SoundStyle(AudioPath.Impact + "MetalHit") {
            Volume = 0.7f,
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 80);

        Projectile.tileCollide = false;
        Projectile.damage = 1;
        Projectile.penetrate = -1;
        Projectile.CritChance = 100;

        Projectile.width = 40;
        Projectile.height = 40;
        Projectile.alpha = 255;
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
            Stick.BeforeVelocity = Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Stick.ToTarget(target, Projectile.Center);
        }

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Projectile.friendly = false;
        Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);

        var invert = Stick.BeforeVelocity.RotatedBy(MathHelper.Pi);
        var start = invert.RotatedBy(-MathHelper.PiOver4);
        var end = invert.RotatedBy(MathHelper.PiOver4);

        var offset = Stick.BeforeVelocity.SafeNormalize(Vector2.Zero);
        Particle.Arc(DustID.Blood, Stick.HitPoint + offset * (Projectile.height / 2), new Vector2(6, 6), start, end, 6,
                     3f, 1.4f);
    }

    public void Push()
    {
        if (Stick.Target != null)
        {
            var shift = Vector2.UnitX.RotatedBy(Stick.BeforeVelocity.ToRotation()) * 6f;
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

        Main.LocalPlayer.GetModPlayer<GoldenPlayer>().RemoveSword(this);

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
            var disappear = Animation.Animate<int>("disappear", 0, 255, 10, Ease.Linear);
            Projectile.alpha = disappear.Update() ?? Projectile.alpha;
        }

        var appear = Animation.Animate<int>("appear", 255, 0, 10, Ease.Linear);
        Projectile.alpha = appear.Update() ?? Projectile.alpha;

        if (appear.Finished)
        {
            if (Stick.Target == null)
            {
                Projectile.velocity = InitialVelocity;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else if (Stick.Target != null)
            {
                var direction = Vector2.UnitX.RotatedBy(Stick.BeforeVelocity.ToRotation()).RotatedBy(MathHelper.Pi);
                var handle = Projectile.Center + direction * Projectile.height;
                handle = handle - (new Vector2(1, 1) * (Size.X / 2));
                WorldRectangle = new Rectangle((int)(handle.X), (int)(handle.Y), (int)Size.X, (int)Size.Y);

                if (Stick.Update() == null)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.Center = Stick.HitPoint + _localShift;
                }
            }
        }
    }
}
