using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;

namespace AquaRegia.Weapons.WoodenWater;

public class WoodenWaterGun : BaseItem
{
    public override string Texture => Assets.Weapons + $"{nameof(WoodenWater)}/{nameof(WoodenWaterGun)}";

    public readonly PropertyModule Property;

    public WoodenWaterGun() : base()
    {
        Property = new PropertyModule(this);

        Composite.AddModule(Property);
    }
}