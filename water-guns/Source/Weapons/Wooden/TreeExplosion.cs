using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Wooden;

public class TreeExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public TreeExplosion()
    {
        var immunity = new ImmunityModule();
        immunity.SetDefaults();
        Composite.AddRuntimeModule(immunity);

        Property = new PropertyModule();

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 76, 66, 1, -1, 0f, 0, 100, false);
        Property.SetTimeLeft(this, 15);
    }
}
