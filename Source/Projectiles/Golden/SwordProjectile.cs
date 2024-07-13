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

    public SoundStyle SlashSound { get; private set; }
    public Vector2 InitialVelocity { get; set; } = Vector2.Zero;

    private Vector2 _localShift = Vector2.Zero;

    public SwordProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
        Stick = new StickModule(this);

        SlashSound = new SoundStyle(AudioPath.Impact + "Slash") {
            Volume = 0.7f,
            PitchVariance = 0.1f,
        };
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 500);

        Projectile.tileCollide = false;
        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 40;
        Projectile.height = 40;
        Projectile.alpha = 255;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (target.getRect().Intersects(Projectile.getRect()) && Stick.Target == null)
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
        Projectile.friendly = true;
        _localShift += Vector2.UnitX.RotatedBy(Stick.BeforeVelocity.ToRotation()) * 8f;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<GoldenPlayer>().RemoveSword(this);
    }

    public override void AI()
    {
        base.AI();

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
