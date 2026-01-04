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
            .Damage(25, 6.5f, DamageClass.Melee)
            .UseStyle(ItemUseStyleID.Shoot, 18, 12)
            .UseSound(SoundID.Item71)
            .Shoot<WoodenTridentProjectile>(AmmoID.None, 3.7f)
            .Hide(true)
            .Rarity(ItemRarityID.Pink)
            .Price(Item.sellPrice(silver: 10));
    }
}