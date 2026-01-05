using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            .Damage(8, 1f, DamageClass.Melee)
            .UseStyle(ItemUseStyleID.Shoot, 28, 28)
            .UseSound(SoundID.Item1)
            .Shoot<WoodenTridentProjectile>(AmmoID.None, 1f)
            .Hide(true)
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(copper: 10));
    }
}