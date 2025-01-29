using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Sea;

public class BubbleExplosion : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }

    public BubbleExplosion()
    {
        Property = new PropertyModule();

        Composite.AddModule(Property);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 20, 20, 1, 1, 0, 0, 100, false);
        Property.SetTimeLeft(this, 15);
    }
}
