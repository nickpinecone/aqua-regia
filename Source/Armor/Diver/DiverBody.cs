using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using AquaRegia.Utils;

namespace AquaRegia.Armor.Diver;

[AutoloadEquip(EquipType.Body)]
public class DiverBody : ModItem
{
    public static readonly int RangePercent = 5;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(RangePercent);
    public static LocalizedText SetBonusText { get; private set; }

    public override string Texture => TexturesPath.Armor + "Diver/DiverBody";

    private Projectile _bubble = null;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        SetBonusText = this.GetLocalization("SetBonus");
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 20;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 6;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetDamage(DamageClass.Ranged) += RangePercent / 100f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == ModContent.ItemType<DiverHead>() && legs.type == ModContent.ItemType<DiverLegs>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = SetBonusText.Value;

        if (Main.keyState.IsKeyDown(Keys.W) && player.velocity.Y > 0)
        {
            player.slowFall = true;

            if (_bubble == null)
            {
                _bubble =
                    Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.Zero,
                                                   ModContent.ProjectileType<FloatBubble>(), 0, 0, player.whoAmI);
            }
        }
        else if (_bubble != null)
        {
            _bubble.Kill();
            _bubble = null;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ItemID.DemoniteBar, 20);
        recipe1.AddIngredient(ItemID.ShadowScale, 15);
        recipe1.AddIngredient(ItemID.Seashell, 3);
        recipe1.AddTile(TileID.WorkBenches);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ItemID.CrimtaneBar, 20);
        recipe2.AddIngredient(ItemID.TissueSample, 15);
        recipe2.AddIngredient(ItemID.Seashell, 3);
        recipe2.AddTile(TileID.WorkBenches);
        recipe2.Register();
    }
}
