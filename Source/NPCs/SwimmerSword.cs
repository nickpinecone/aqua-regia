using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WaterGuns.Utils;

namespace WaterGuns.NPCs;

public class SwimmerSword : ModProjectile
{
    public override string Texture => TexturesPath.NPCs + "PoolNoodle";

    public Timer AttackTimer { get; private set; }

    public override void SetDefaults()
    {
        AttackTimer = new Timer(4, true);

        base.SetDefaults();

        Projectile.damage = 1;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 2;

        Projectile.width = 64;
        Projectile.height = 8;

        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.hide = true;
    }

    int dir = 1;
    public override void AI()
    {
        base.AI();

        AttackTimer.Update();

        if (Main.mouseRight)
        {
            if (AttackTimer.Done)
            {
                AttackTimer.Restart();

                var velocity = new Vector2(1, 0).RotatedBy(Main.LocalPlayer.itemRotation);
                velocity *= 12f;

                // Projectile.NewProjectile(Projectile.GetSource_None(), Main.LocalPlayer.Center, velocity,
                //                          ModContent.ProjectileType<SwimmerProjectile>(), 1, 1, Projectile.owner);
            }

            Projectile.Center = Main.LocalPlayer.Center + (new Vector2(32, 0)).RotatedBy(Main.LocalPlayer.itemRotation);
            Projectile.rotation = Main.LocalPlayer.itemRotation - 0.4f;

            Main.LocalPlayer.itemAnimation = 2;
            Main.LocalPlayer.itemTime = 2;

            if (Math.Abs(Main.LocalPlayer.itemRotation) > MathHelper.PiOver2)
            {
                dir = -dir;
            }
            Main.LocalPlayer.itemRotation += 0.15f * dir;

            Projectile.timeLeft = 2;
        }
    }

    public override void DrawBehind(int index, System.Collections.Generic.List<int> behindNPCsAndTiles,
                                    System.Collections.Generic.List<int> behindNPCs,
                                    System.Collections.Generic.List<int> behindProjectiles,
                                    System.Collections.Generic.List<int> overPlayers,
                                    System.Collections.Generic.List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }
}
