using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.Accessoires;

[AutoloadEquip(EquipType.Face)]
public class WeirdFrogHat : ModItem
{
    public override string Texture => TexturesPath.Accessories + "WeirdFrogHat";
    public Timer AttackTimer { get; private set; }

    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 40;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Green;

        AttackTimer = new Timer(120, true);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        AttackTimer.Update();

        if (AttackTimer.Done)
        {
            var target = Helper.FindNearsetNPC(Main.LocalPlayer.Top, 512f);

            if (target != null)
            {
                AttackTimer.Restart();

                var direction = target.Center - Main.LocalPlayer.Top;
                direction.Normalize();
                direction *= 24f;

                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Main.LocalPlayer.Top, direction,
                                               ModContent.ProjectileType<TongueProjectile>(), 12, 1f, Main.myPlayer);
            }
        }
    }
}
