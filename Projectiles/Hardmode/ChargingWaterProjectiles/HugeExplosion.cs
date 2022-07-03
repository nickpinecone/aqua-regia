using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace WaterGuns.Projectiles.Hardmode.ChargingWaterProjectiles
{
    public class HugeExplosion : BaseProjectile
    {
        int width = 256;
        int height = 256;
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = ProjectileID.WaterGun;
            Projectile.height = height;
            Projectile.width = width;
            Projectile.timeLeft = 12;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14);

            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (MathF.Abs(Main.npc[i].position.X - Projectile.Center.X) < 196 && MathF.Abs(Main.npc[i].position.Y - Projectile.Center.Y) < 196)
                    Main.npc[i].AddBuff(BuffID.Confused, 240);
            }
            for (int i = 0; i < 32; i++)
            {
                var rotation = Main.rand.Next(0, 360);
                var velocity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(rotation));
                velocity *= 3.6f;
                var dust = Dust.NewDustDirect(Projectile.Center, 16, 16, DustID.Wet, velocity.X, velocity.Y);
                dust.scale = 3f;
            }
            base.Kill(timeLeft);
        }
    }
}
