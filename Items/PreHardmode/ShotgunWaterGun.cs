using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace WaterGuns.Items.PreHardmode
{
    public class ShotgunWaterGun : BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Blunderbuss");
            Tooltip.SetDefault("Shoots multiple streams of water\nFull Pump: Significantly increases knockback, but the holder is thrown back from strong recoil\nBought from Swimmer after defeating King Slime");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            base.offsetAmount = new Vector2(5, 5);
            base.offsetIndependent = new Vector2(0, -4);

            Item.damage = 20;
            Item.knockBack = 6;

            Item.useAnimation += 12;
            Item.useTime += 12;

            Item.value = Item.buyPrice(0, 5, 25, 0);

            base.defaultInaccuracy = 12;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (pumpLevel >= maxPumpLevel)
            {
                // Recoil
                Main.player[Main.myPlayer].velocity += new Vector2(-velocity.X / 1.5f, -velocity.Y * 1.5f);
                knockback = 12;
            }

            // So it applies to all shots, not the first one only
            var _pumpLevel = pumpLevel;

            for (int i = -1; i < 3; i++)
            {
                pumpLevel = _pumpLevel;
                int distanceBetween = Main.rand.Next(6, 10);
                Vector2 modifiedVelocity = velocity.RotatedBy(MathHelper.ToRadians(distanceBetween * i * player.direction));
                var proj = base.SpawnProjectile(player, source, position, modifiedVelocity, type, damage, knockback);
                proj.timeLeft -= Main.rand.Next(10, 30);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddCondition(NetworkText.FromLiteral("Mods.WaterGuns.Conditions.Never"), (_) => false);
            recipe.Register();
        }
    }
}
