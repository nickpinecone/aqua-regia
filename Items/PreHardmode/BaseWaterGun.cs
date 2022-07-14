using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Items.PreHardmode
{
    public abstract class BaseWaterGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaterGun);
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>();
            Item.useAmmo = ItemID.BottledWater;
        }

        public float CalculateAccuracy(float inaccuracy = 3.3f)
        {
            return Main.player[Main.myPlayer].GetModPlayer<GlobalPlayer>().CalculateAccuracy(inaccuracy);
        }

        public float CalculateSpeed()
        {
            return Main.player[Main.myPlayer].GetModPlayer<GlobalPlayer>().CalculateSpeed();
        }

        protected bool isOffset = false;
        protected float defaultInaccuracy = 3.3f;
        protected Vector2 offsetAmount = new Vector2(4, 4);
        protected Vector2 offsetIndependent = new Vector2(0, 0);
        public void SpawnProjectile(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float inaccuracy = CalculateAccuracy();
            // All of them use custom projectiles that shoot straight 
            // Make them a little inaccurate like in-game water gun
            Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(inaccuracy)) * CalculateSpeed();
            // Offset if need be
            var offset = isOffset ? new Vector2(position.X + velocity.X * offsetAmount.X, position.Y + velocity.Y * offsetAmount.Y) : position;
            var proj = Projectile.NewProjectileDirect(source, offset + offsetIndependent, modifiedVelocity, Item.shoot, damage, knockback, player.whoAmI);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SpawnProjectile(player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            // So holding guns look like in-game water gun
            return new Vector2(0, 4);
        }
    }
}
