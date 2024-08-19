using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sunflower;

public class SeedProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sunflower/SeedProjectile";

    public PropertyModule Property { get; private set; }

    public SeedProjectile() : base()
    {
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);

        Projectile.damage = 1;
        Projectile.knockBack = 1;
        Projectile.penetrate = 1;

        Projectile.width = 8;
        Projectile.height = 8;

        Projectile.tileCollide = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
    }
}
