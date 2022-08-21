using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace WaterGuns.NPCs
{
    public class Swimmer_Projectile : Projectiles.PreHardmode.BaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.position += Projectile.velocity * 2;
            base.OnSpawn(source);
        }
    }
}
