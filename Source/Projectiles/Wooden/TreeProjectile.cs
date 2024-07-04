using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Wooden;

public class TreeProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "TreeProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }

    public TreeProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);

        Projectile.friendly = false;
        Projectile.damage = 1;
        Projectile.knockBack = 1f;
        Projectile.penetrate = -1;

        Projectile.width = 76;
        Projectile.height = 66;
        Projectile.gfxOffY = 38;
        Projectile.alpha = 255;
    }

    bool didCollide = false;
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        didCollide = true;

        SoundEngine.PlaySound(SoundID.Item14);

        foreach (var particle in Particle.Arc(DustID.Cloud, Projectile.Bottom, new Vector2(2, 2),
                                              Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-150)),
                                              Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-30)), 6, 8f, 5f, 0, 75))
        {
            particle.noGravity = true;
        }

        return base.OnTileCollide(oldVelocity);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        if (!didCollide)
        {
            SoundEngine.PlaySound(SoundID.Grass);

            Particle.Circle(DustID.GrassBlades, Projectile.Center, new Vector2(12, 12), 6, 2f, 1f);
        }
    }

    float rotState = 0;
    public override void AI()
    {
        base.AI();

        // TODO rewrite after animation overhaul

        Projectile.alpha = (int)(Animation.AnimateF("appear", 255, 0, 10, new string[] { }) ?? Projectile.alpha);

        Projectile.rotation = Animation.AnimateF("rot", 0f, -1f, 8, new string[] { "appear" }) ?? Projectile.rotation;

        if (!Animation.IsFinished("rot"))
        {
            Projectile.velocity += (new Vector2(-0.8f, 0)).RotatedBy(MathHelper.ToRadians(45));
        }
        else
        {
            if (!Animation.IsFinished("rotSlow"))
            {
                var rotationSlow = Animation.AnimateF("rotSlow", 0.1f, 0, 10, new string[] { }) ?? 0;
                Projectile.rotation -= rotationSlow;
                Projectile.velocity =
                    Animation.AnimateVec("slow", Projectile.velocity, Vector2.Zero, 10, new string[] { }) ??
                    Projectile.velocity;
                rotState = Projectile.rotation;
            }
        }

        if (Animation.IsFinished("slow"))
        {
            Projectile.friendly = true;

            Projectile.rotation =
                Animation.AnimateF("finalRot", rotState, MathHelper.ToRadians(135), 25, new string[] { }) ??
                Projectile.rotation;
            Projectile.velocity += (new Vector2(0.8f, 0)).RotatedBy(MathHelper.ToRadians(45));
        }

        if (Animation.IsFinished("finalRot"))
        {
            Projectile.Kill();
        }
    }
}
