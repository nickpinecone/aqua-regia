using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Wooden;

// TODO randomize stuff and make different directions

public class TreeProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "TreeProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }

    private bool _didCollide = false;

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

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        _didCollide = true;

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

        if (!_didCollide)
        {
            SoundEngine.PlaySound(SoundID.Grass);

            Particle.Circle(DustID.GrassBlades, Projectile.Center, new Vector2(12, 12), 6, 2f, 1f);
        }
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = MathHelper.ToRadians(-30);
        Projectile.position -= new Vector2(90, 160);
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha =
            (int)(Animation.AnimateF("appear", 255, 0, 6, new string[] {}, Easing.Linear) ?? Projectile.alpha);

        if (Animation.IsFinished("appear"))
        {
            Projectile.friendly = true;
        }

        Projectile.rotation = Animation.AnimateF("rot", MathHelper.ToRadians(-30), MathHelper.ToRadians(110), 20,
                                                 new string[] { "appear" }, Easing.InOut) ??
                              Projectile.rotation;

        Projectile.velocity =
            Animation.AnimateVec("vel", Vector2.Zero, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(60)) * 20f, 20,
                                 new string[] { "appear" }, Easing.InOut) ??
            Projectile.velocity;

        if (Animation.IsFinished("rot"))
        {
            Projectile.Kill();
        }
    }
}
