using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.NPCs
{
    public class Swimmer_Gun : Items.PreHardmode.BaseWaterGun
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Swimmer Gun");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<Projectiles.PreHardmode.SimpleWaterProjectile>();

            Item.damage = 1;
            base.isOffset = true;
            base.offsetAmount = new Vector2(3f, 3f);
            base.offsetIndependent = new Vector2(0, -4f);

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, -1);
        }
    }
}
