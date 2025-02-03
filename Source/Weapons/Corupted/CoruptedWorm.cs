using AquaRegia.Modules.Mobs;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Weapons.Corupted;

public class CoruptedWormHead : BaseProjectile
{
    public static int SegmentSpace => 16;
    public static int SegmentAmount => 5;

    public override string Texture => TexturesPath.Weapons + "Corupted/WormHead";

    public WormModule Worm { get; private set; }
    public Modules.Projectiles.PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }

    public CoruptedWormHead()
    {
        Worm = new WormModule();
        Property = new Modules.Projectiles.PropertyModule();
        Home = new HomeModule();

        Composite.AddModule(Worm, Property, Home);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Home.SetDefaults(curveChange: 1f);
        Property.SetProperties(this, 28, 28, 10, -1, 1f, tileCollide: false);
        Property.SetTimeLeft(this, 600);
        Worm.SetDefaults(WormBodyType.Head, null, SegmentAmount, SegmentSpace);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Logger.Message("I was spawned");

        Worm.SpawnSegments<CoruptedWormBody, CoruptedWormBody>(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Projectile.damage, Projectile.knockBack);
        Projectile.velocity = Vector2.UnitX * 8;
    }

    public override void AI()
    {
        base.AI();

        Worm.Follow(Projectile.Center, Projectile.rotation);
        Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, Main.MouseWorld);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
    }
}

public class CoruptedWormBody : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Corupted/WormBody";

    public WormModule Worm { get; private set; }
    public Modules.Projectiles.PropertyModule Property { get; private set; }

    public CoruptedWormBody()
    {
        Worm = new WormModule();
        Property = new Modules.Projectiles.PropertyModule();

        Composite.AddModule(Worm, Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 28, 28, 10, -1, 1f, tileCollide: false);
        Property.SetTimeLeft(this, 600);
        Worm.SetDefaults(WormBodyType.Body, null, CoruptedWormHead.SegmentAmount, CoruptedWormHead.SegmentSpace);
    }

    public override void AI()
    {
        base.AI();

        var (position, rotation) = Worm.Follow(Projectile.Center, Projectile.rotation);
        Projectile.Center = position;
        Projectile.rotation = rotation;
    }
}