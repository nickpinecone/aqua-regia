using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Wooden;

public class TreeProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "TreeProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }

    private bool _didCollide = false;
    private int _direction = 0;

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
        Projectile.CritChance = 100;

        Projectile.width = 76;
        Projectile.height = 66;
        Projectile.gfxOffY = 38;
        Projectile.alpha = 255;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        _didCollide = true;

        Main.player[Projectile.owner].GetModPlayer<ScreenShake>().Activate(6, 2);

        SoundEngine.PlaySound(SoundID.Item14);

        foreach (var particle in Particle.Arc(DustID.Cloud, Projectile.Bottom, new Vector2(2, 2),
                                              Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-150)),
                                              Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-30)), 6, 9f, 5f, 0, 75))
        {
            particle.noGravity = true;
        }

        return base.OnTileCollide(oldVelocity);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Particle.Circle(DustID.GrassBlades, Projectile.Center, new Vector2(12, 12), 6, 2f, 1f);

        if (!_didCollide)
        {
            SoundEngine.PlaySound(SoundID.Grass);
        }
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _direction = Main.rand.NextFromList(new int[] { -1, 1 });

        Projectile.rotation = MathHelper.ToRadians(-30 * _direction);
        Projectile.position -= new Vector2(90 * _direction, 160);
    }

    public override void AI()
    {
        base.AI();

        var appear = Animation.Animate<int>("appear", 255, 0, 6, Ease.Linear);
        Projectile.alpha = appear.Value ?? Projectile.alpha;

        if (appear.Finished)
        {
            Projectile.friendly = true;
        }

        var rot =
            Animation.Animate<float>("rot", MathHelper.ToRadians(-30 * _direction),
                                     MathHelper.ToRadians(110 * _direction), 20, Ease.InOut, new string[] { "appear" });
        Projectile.rotation = rot.Value ?? Projectile.rotation;

        var vel = Animation.Animate<Vector2>("vel", Vector2.Zero,
                                             Vector2.UnitY.RotatedBy(MathHelper.ToRadians(-30 * _direction)) * 18f, 20,
                                             Ease.InOut, new string[] { "appear" });
        Projectile.velocity = vel.Value ?? Projectile.velocity;

        if (rot.Finished)
        {
            Projectile.Kill();
        }
    }
}
