using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Corupted;

public class CoruptedWormHead : BaseProjectile
{
    public static int SegmentSpace => 16;
    public static int SegmentAmount => 5;

    public override string Texture => TexturesPath.Weapons + "Corupted/WormHead";

    public WormModule<CoruptedWormBody, CoruptedWormBody> Worm { get; private set; }
    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }

    public CoruptedWormHead()
    {
        Worm = new WormModule<CoruptedWormBody, CoruptedWormBody>();
        Property = new PropertyModule();
        Home = new HomeModule();

        Composite.AddModule(Worm, Property, Home);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Home.SetDefaults(curveChange: 1f, speed: 2);
        Property.SetProperties(this, 28, 28, 10, -1, 1f, tileCollide: false);
        Property.SetTimeLeft(this, 600);
        Worm.SetDefaults(SegmentAmount, SegmentSpace);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Logger.Message("I was spawned");

        Worm.SpawnSegments(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Projectile.damage, Projectile.knockBack);
        Projectile.velocity = Vector2.UnitX * 8;
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, Main.MouseWorld);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        Worm.PostUpdate(this, Projectile.velocity);
    }
}

public class CoruptedWormBody : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Corupted/WormBody";

    public PropertyModule Property { get; private set; }

    public CoruptedWormBody()
    {
        Property = new PropertyModule();

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 28, 28, 10, -1, 1f, tileCollide: false);
        Property.SetTimeLeft(this, 600);
    }
}