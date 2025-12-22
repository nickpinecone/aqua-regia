using AquaRegia.Ammo;
using AquaRegia.Library;
using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Attributes;
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
    public override string Texture => Assets.Sprites.Weapons.WoodenWater.WoodenWaterGun;

    private PropertyModule Property { get; } = new();
    private TreeBoostModule TreeBoost { get; } = new();
    private WaterModule Water { get; } = new();

    [RuntimeModule] private SpriteModule Sprite { get; } = new();
    [RuntimeModule(1)] private AccuracyModule Accuracy { get; } = new();
    [RuntimeModule] private ProgressModule Progress { get; } = new();

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sprite.SetOffsets(new Vector2(26f, 26f), new Vector2(0, 6));
        Water.SetUseSound(this);
        Progress.SetTimer(Tween.Create<int>(5 * 60));
        Accuracy.SetInaccuracy(3.5f);

        Property.Set(this)
            .Size(38, 22)
            .Damage(4, 0.8f, DamageClass.Ranged)
            .UseStyle(ItemUseStyleID.Shoot, 20, 20)
            .Shoot<WoodenWaterProjectile>(ModContent.ItemType<BottledWater>(), 22f)
            .Rarity(ItemRarityID.White)
            .Price(Item.sellPrice(0, 0, 0, 20));

        TreeBoost.SetDefaults(Item.damage, 2);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 20)
            .AddIngredient(ItemID.Acorn, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        Item.damage = TreeBoost.Apply(player);
    }

    public override void AltUseAlways(Player player)
    {
        base.AltUseAlways(player);

        if (Progress.Timer.Done)
        {
            new ProjectileSpawner<TreeProjectile>()
                .Context(new WeaponWithAmmoSource(this), player)
                .Position(Main.MouseWorld)
                .Damage(Item.damage * 2, Item.knockBack * 2)
                .Spawn();

            Progress.Timer.Restart();
        }
    }
}