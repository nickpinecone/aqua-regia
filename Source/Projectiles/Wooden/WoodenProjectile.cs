
using WaterGuns.Projectiles.Modules;

namespace WaterGuns.Projectiles.Wooden;

public class WoodenProjectile : BaseProjectile
{
    public override string Texture => "WaterGuns/Assets/Textures/Empty";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }
    public VisualModule Visual { get; private set; }

    public WoodenProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
        Visual = new VisualModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults();
        Visual.SetDefaults();
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Visual.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
