using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Players;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Wooden;

public class TreeProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Wooden/TreeProjectile";

    public PropertyModule Property { get; private set; }
    public Animation<int> Appear { get; private set; }
    public Animation<float> Rotate { get; private set; }
    public Animation<Vector2> Velocity { get; private set; }

    private bool _didCollide = false;
    private int _direction = 0;

    public TreeProjectile() : base()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();

        Appear = new Animation<int>(6);
        Rotate = new Animation<float>(20, Ease.InOut, new BaseAnimation[] { Appear });
        Velocity = new Animation<Vector2>(20, Ease.InOut, new BaseAnimation[] { Appear });

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 76, 66, 1, -1, 1f, 255, 0, true, false, false);
        Property.SetTimeLeft(this, 120);

        Projectile.gfxOffY = 38;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (!Rotate.Finished)
        {
            _didCollide = true;

            Owner.GetModPlayer<ScreenShake>().Activate(6, 2);

            SoundEngine.PlaySound(SoundID.Item14);
            Helper.SpawnProjectile<TreeExplosion>(Projectile.GetSource_FromThis(), Owner, Projectile.Center,
                                                  Vector2.Zero, Projectile.damage, Projectile.knockBack);

            foreach (var particle in Particle.Arc(DustID.Cloud, Projectile.Bottom, new Vector2(2, 2),
                                                  Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-150)),
                                                  Vector2.UnitX.RotatedBy(MathHelper.ToRadians(-30)), 6, 9f, 5f, 0, 75))
            {
                particle.noGravity = true;
            }
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

        if (TileHelper.AnySolidInArea(Projectile.Center, 3, 3))
        {
            Projectile.Kill();
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;

        if (Appear.Finished)
        {
            Projectile.friendly = true;
        }

        Projectile.rotation =
            Rotate.Animate(MathHelper.ToRadians(-30 * _direction), MathHelper.ToRadians(110 * _direction)) ??
            Projectile.rotation;

        Projectile.velocity =
            Velocity.Animate(Vector2.Zero, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(-30 * _direction)) * 18f) ??
            Projectile.velocity;

        if (Rotate.Finished)
        {
            Projectile.Kill();
        }
    }
}
