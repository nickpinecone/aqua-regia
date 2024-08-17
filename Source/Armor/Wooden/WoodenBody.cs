using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using AquaRegia.Utils;

namespace AquaRegia.Armor.Wooden;

[AutoloadEquip(EquipType.Body)]
public class WoodenBody : ModItem
{
    public static readonly int RangePercent = 3;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(RangePercent);
    public static LocalizedText SetBonusText { get; private set; }

    public override string Texture => TexturesPath.Armor + "Wooden/WoodenBody";

    public Timer RootTimer { get; private set; }
    public Timer HealTimer { get; private set; }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        SetBonusText = this.GetLocalization("SetBonus");
    }

    public override void SetDefaults()
    {
        Item.width = 25;
        Item.height = 25;
        Item.value = Item.sellPrice(silver: 1);
        Item.rare = ItemRarityID.White;
        Item.defense = 2;

        RootTimer = new Timer(30, true);
        HealTimer = new Timer(300, true);
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetDamage(DamageClass.Ranged) += RangePercent / 100f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == ModContent.ItemType<WoodenHead>() && legs.type == ModContent.ItemType<WoodenLegs>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = SetBonusText.Value;

        var tilePosition = (player.Bottom + new Vector2(0, 12)).ToTileCoordinates();

        if (player.GetModPlayer<WoodenPlayer>().Root == null && TileHelper.IsSolid(tilePosition) &&
            Main.LocalPlayer.velocity.Length() <= 1e-3)
        {
            RootTimer.Update();
            HealTimer.Restart();

            if (RootTimer.Done)
            {
                RootTimer.Restart();

                var directions = new int[] { 1, -1 };

                for (int i = 0; i < directions.Length; i++)
                {
                    var source = new WoodenSource(Projectile.GetSource_NaturalSpawn());
                    source.Direction = directions[i];
                    Projectile.NewProjectileDirect(source, Main.LocalPlayer.Bottom + new Vector2(0, 8), Vector2.Zero,
                                                   ModContent.ProjectileType<RootProjectile>(), 0, 0, Main.myPlayer);
                }
            }
        }
        else if (player.GetModPlayer<WoodenPlayer>().Root != null)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;

            RootTimer.Restart();
            HealTimer.Update();

            if (HealTimer.Done)
            {
                HealTimer.Restart();
                player.Heal(1);
            }
        }
        else
        {
            RootTimer.Restart();
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Wood, 25);
        recipe.AddIngredient(ItemID.Rope, 15);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
