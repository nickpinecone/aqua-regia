using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using AquaRegia.Library.Extended.Modules.Shared;
using AquaRegia.Library.Tween;
using Terraria;

namespace AquaRegia.Weapons.WoodenWater;

public class WoodenWaterGun : BaseItem
{
    public override string Texture => Assets.Weapons + $"{nameof(WoodenWater)}/{nameof(WoodenWaterGun)}";

    public readonly PropertyModule Property;
    public readonly ProgressModule Pump;

    public WoodenWaterGun() : base()
    {
        Property = new PropertyModule(this);
        Pump = new ProgressModule();

        Composite.AddModule(Property, Pump);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Pump.Initialize(TweenManager.Create<int>(5 * 60));
        Property.Size(38, 22);
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        Pump.Update();
    }

    public override void AltUseAlways(Player player)
    {
        base.AltUseAlways(player);

        if (Pump.Done)
        {
            Pump.Reset();
        }
    }
}