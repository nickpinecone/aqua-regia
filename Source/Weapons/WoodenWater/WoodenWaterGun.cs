using AquaRegia.Ammo;
using AquaRegia.Library;
using AquaRegia.Library.Extended.Helpers;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using AquaRegia.Library.Extended.Modules.Sources;
using AquaRegia.Library.Tween;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenWater;

public class WoodenWaterGun : BaseItem
{
    public override string Texture => Assets.Weapons + $"{nameof(WoodenWater)}/{nameof(WoodenWaterGun)}";

    private PropertyModule Property { get; }
    private TreeBoostModule TreeBoost { get; }
    private SpriteModule Sprite { get; }
    private WaterModule Water { get; }
    private AccuracyModule Accuracy { get; }
    private ProgressModule Progress { get; }

    public WoodenWaterGun()
    {
        Property = new PropertyModule(this);
        TreeBoost = new TreeBoostModule();
        Sprite = new SpriteModule();
        Water = new WaterModule();
        Accuracy = new AccuracyModule();
        Progress = new ProgressModule();

        Composite.AddModule(Property, TreeBoost, Sprite, Water, Accuracy, Progress);
        Composite.AddRuntimeModule(Sprite, Accuracy);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sprite.SetDefaults(new Vector2(26f, 26f), new Vector2(0, 6));
        Water.SetUseSound(this);
        Progress.SetDefaults(Tween.Create<int>(5 * 60));
        Accuracy.SetInaccuracy(3.5f);

        Property.Size(38, 22)
            .Damage(4, 0.8f, DamageClass.Ranged)
            .UseStyle(ItemUseStyleID.Shoot, 20, 20)
            .Shoot<WoodenWaterProjectile>(ModContent.ItemType<BottledWater>(), 22f)
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(0, 0, 0, 20));

        TreeBoost.SetDefaults(Item.damage, 2);
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        Item.damage = TreeBoost.Apply(player);
        Progress.Timer.Delay();
    }

    public override void AltUseAlways(Player player)
    {
        base.AltUseAlways(player);

        if (Progress.Timer.Done)
        {
            var weaponSource = new WeaponWithAmmoSource(this);

            ModHelper.SpawnProjectile<TreeProjectile>(weaponSource, player, Main.MouseWorld, Vector2.Zero,
                Item.damage * 2, Item.knockBack * 2);

            Progress.Timer.Restart();
        }
    }
}