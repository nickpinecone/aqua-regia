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
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (target.getRect().Intersects(Projectile.getRect()) && Stick.Target == null)
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
            var temp = Vector2.UnitX.RotatedBy(Stick.BeforeVelocity.ToRotation()) * 6f;
            var rect = new Rectangle((int)(Projectile.position.X + temp.X), (int)(Projectile.position.Y + temp.Y),
                                     Projectile.width, Projectile.height);

            if (rect.Intersects(Stick.Target.getRect()))
            {
                SoundEngine.PlaySound(MetalHitSound);

                _localShift += temp;
                Projectile.damage = (int)(Projectile.damage * 1.1f);
                Projectile.friendly = true;
            }
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<GoldenPlayer>().RemoveSword(this);
    }

    public override void AI()
    {
        base.AI();

        var size = new Vector2(Projectile.width * 1.5f, Projectile.height * 1.5f);
        WorldRectangle = new Rectangle((int)(Projectile.Center.X - size.X / 2), (int)(Projectile.Center.Y - size.Y / 2),
                                       (int)size.X, (int)size.Y);

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
