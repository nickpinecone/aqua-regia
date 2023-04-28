using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Buffs
{
    public class BubbleWhirlDebuff : ModBuff
    {
        int delay = 6;
        public override void Update(NPC npc, ref int buffIndex)
        {
            base.Update(npc, ref buffIndex);

            delay += 1;
            if (delay > 2)
            {
                // Offset randomly
                var offset = new Vector2();
                offset.X = npc.position.X + Main.rand.Next(-60, 60);
                offset.Y = npc.Bottom.Y - Main.rand.Next(0, 60);


                // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), offset, new Vector2(0, -6), ModContent.ProjectileType<Projectiles.PreHardmode.BubbleProjectile>(), 32, 0, Main.myPlayer);
                delay = 0;
            }

        }
    }
}
