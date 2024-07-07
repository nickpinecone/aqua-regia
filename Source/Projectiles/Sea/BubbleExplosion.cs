
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class BubbleExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public BubbleExplosion()
    {
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 15);

        Projectile.tileCollide = false;
        Projectile.damage = 1;
        Projectile.penetrate = 1;
        Projectile.CritChance = 100;

        // Only damage one NPC so hitbox is small
        Projectile.width = 20;
        Projectile.height = 20;
    }
}
