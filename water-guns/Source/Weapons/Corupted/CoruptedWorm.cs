using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Corupted;

public class CoruptedWormHead : BaseProjectile
{
    public static int SegmentSpace => 14;
    public static int SegmentAmount => 6;

    public override string Texture => TexturesPath.Weapons + "Corupted/WormHead";

    public WormModule<CoruptedWormBody, CoruptedWormTail> Worm { get; private set; }
    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }

    public CoruptedWormHead()
    {
        Worm = new();
        Property = new PropertyModule();
        Home = new HomeModule();

        Composite.AddModule(Worm, Property, Home);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Home.SetDefaults(curve: 0.15f, curveChange: 1f, speed: 6);
        Property.SetProperties(this, 28, 28, 10, -1, 1f, tileCollide: false);
        Property.SetTimeLeft(this, 600);
        Worm.SetDefaults(SegmentAmount, SegmentSpace);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Worm.SpawnSegments(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Projectile.damage, Projectile.knockBack);
        Projectile.velocity = Vector2.UnitX * 8;
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, Main.MouseWorld);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        Worm.PostUpdate(Projectile.Center);
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

public class CoruptedWormTail : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Corupted/WormTail";

    public PropertyModule Property { get; private set; }

    public CoruptedWormTail()
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