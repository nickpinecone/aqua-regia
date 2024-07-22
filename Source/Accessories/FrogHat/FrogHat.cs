using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Accessories.FrogHat;
using WaterGuns.Utils;

namespace WaterGuns.Accessoires.FrogHat;

public class WeirdFrogHat : ModItem
{
    public override string Texture => TexturesPath.Accessories + "FrogHat/FrogHat";
    public Timer AttackTimer { get; private set; }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 14;
        Item.accessory = true;
        Item.scale = 0.5f;
        Item.value = Item.buyPrice(gold: 1);
        Item.rare = ItemRarityID.Blue;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        Main.LocalPlayer.GetModPlayer<FrogPlayer>().Active = true;

        if (Main.LocalPlayer.GetModPlayer<FrogPlayer>().Minion == null)
        {
            Projectile.NewProjectile(Projectile.GetSource_None(), Main.LocalPlayer.Center, Vector2.Zero,
                                     ModContent.ProjectileType<FrogMinion>(), 12, 1f, Main.myPlayer);
        }
    }
}
