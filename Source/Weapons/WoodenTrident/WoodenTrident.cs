using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using Terraria;
using Terraria.ID;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTrident : BaseItem
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.wooden_trident;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private SpearModule Spear { get; } = new();
    [RuntimeModule] private RecallModule<WoodenTridentThrow> Recall { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Recall.SetDefaults(12f);

        Property.Set(this)
            .Defaults.Spear()
            .Damage(8, 1f)
            .UseTime(28, 28)
            .Shoot<WoodenTridentProjectile>()
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(copper: 10));
    }
}