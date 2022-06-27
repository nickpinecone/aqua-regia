using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace WaterGuns.Debuffs
{
    public class BubbleSuffocateDebuff : ModBuff
    {
        int delay = 0;
        public override void Update(NPC npc, ref int buffIndex)
        {
            base.Update(npc, ref buffIndex);

            if (!npc.boss) npc.velocity = new Vector2(0, -1);
            npc.lifeRegen -= 32;

            delay += 1;
            if (delay > 6)
            {
                // Offset randomly
                var offset = new Vector2();
                offset.X = npc.position.X + Main.rand.Next(-60, 60);
                offset.Y = npc.position.Y + npc.height + Main.rand.Next(0, 60);

                // Dont know how to extract IEventSource from BubbleProjectile so using OceanWaterProjectile source
                Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), offset, new Vector2(0, -6), ModContent.ProjectileType<Projectiles.PreHardmode.BubbleProjectile>(), 28, 0, Main.myPlayer);
                delay = 0;
            }

        }
    }
}
