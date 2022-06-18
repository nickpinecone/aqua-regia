using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Projectiles.PreHardmode
{
    public class IceWaterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.IceBolt);
            AIType = ProjectileID.IceBolt;
            Projectile.friendly = false;
        }

        Vector2 spin = new Vector2(24, 0);
        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                base.AI();
            }
            else
            {
                spin = spin.RotatedBy(MathHelper.ToRadians(1.6f));
                Projectile.position = Main.player[Main.myPlayer].position + spin;
            }
        }
    }
}
