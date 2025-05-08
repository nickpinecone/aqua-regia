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

    private float _curveAmount = 0f;

    public CoruptedWormHead()
    {
        Composite.AddRuntimeModule(ImmunityModule.New());

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
        Property.SetTimeLeft(this, 2);
        Worm.SetDefaults(SegmentAmount, SegmentSpace);

        _curveAmount = Home.Curve;
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        return Worm.Colliding(projHitbox, targetHitbox);
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        Worm.SpawnSegments(Projectile.GetSource_FromThis(), Owner, Projectile.Center, Projectile.damage, Projectile.knockBack);
        Owner.GetModPlayer<CoruptedPlayer>().AddWorm(this);
    }

    public override void AI()
    {
        base.AI();

        var target = Helper.FindNearestNPC(Projectile.Center, 600f);
        var targetPos = target?.Center ?? Main.MouseWorld;

        Home.Curve = _curveAmount + Main.rand.NextFloat(-0.1f, 0.1f);
        Projectile.velocity = Home.Calculate(Projectile.Center, Projectile.velocity, targetPos);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        Worm.PostUpdate(Projectile.Center, Projectile.timeLeft);
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

        Property.SetProperties(this, 28, 28, 10, -1, 1f, friendly: false, tileCollide: false);
        Property.SetTimeLeft(this, 2);
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

        Property.SetProperties(this, 28, 28, 10, -1, 1f, friendly: false, tileCollide: false);
        Property.SetTimeLeft(this, 2);
    }
}