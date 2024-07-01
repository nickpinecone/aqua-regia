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

        Projectile.damage = 1;
        Projectile.knockBack = 1f;
        Projectile.penetrate = -1;

        Projectile.width = 76;
        Projectile.height = 66;
        Projectile.gfxOffY = 38;
        Projectile.alpha = 255;
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = (int)(Animation.AnimateF("appear", 255, 0, 10, new string[] {}) ?? Projectile.alpha);

        Projectile.rotation = Animation.AnimateF("rot", 0f, -1f, 15, new string[] { "appear" }) ?? Projectile.rotation;
    }
}
