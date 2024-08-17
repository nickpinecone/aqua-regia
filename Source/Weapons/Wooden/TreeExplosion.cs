using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Wooden;

public class TreeExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public TreeExplosion()
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
        Projectile.penetrate = -1;
        Projectile.CritChance = 100;

        Projectile.width = 76;
        Projectile.height = 66;
    }
}
