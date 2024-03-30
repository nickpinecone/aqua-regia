using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace WaterGuns.Pets
{
    public class DuckPetProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;

            // This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
            // *It uses fluent API syntax, just like Recipe
            // *You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
            // * To stop the animation if the player is not highlighted /is standing, as done by most grounded pets, add a.WhenNotSelected(0, 0)(you can customize it just like SimpleLoop)
            // * To set offset and direction, use .WithOffset(x, y) and.WithSpriteDirection(-1)
            // * To further customize the behavior and animation of the pet(as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode().You can also make your own, showcased in MinionBossPetProjectile
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(3, 8, 4)
                .WhenNotSelected(0, 0)
                .WithOffset(-15, 0f)
                .WithSpriteDirection(-1)
                .WithCode(DelegateMethods.CharacterPreview.BerniePet);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyDino);

            Projectile.width = 48;
            Projectile.height = 36;

            AIType = ProjectileID.BabyDino;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.dino = false;

            return true;
        }

        int walkCounter = 0;
        public override void AI()
        {
            var tilePosition = (Projectile.position + new Vector2(0, 24)).ToTileCoordinates();
            var tile = Main.tile[tilePosition.X, tilePosition.Y];

            if (tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Water)
            {
                if (Projectile.velocity.Y > 0)
                {
                    Projectile.velocity.Y = 0;
                }

                Projectile.frame = 1;
            }
            else
            {
                // Flying code, for later reference
                // if (Projectile.velocity.Y <= -0.5f)
                // {
                //     if (Projectile.frame < 11 || Projectile.frame > 14)
                //         Projectile.frame = 11;
                //     if (++flyCounter >= 6)
                //     {
                //         flyCounter = 0;
                //         // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                //         if (++Projectile.frame >= 14)
                //             Projectile.frame = 11;
                //     }
                // }
                // else
                // {
                if (Math.Abs(Projectile.velocity.X) >= 0.5f)
                {
                    if (Projectile.frame < 3 || Projectile.frame > 10)
                        Projectile.frame = 3;

                    if (++walkCounter >= 6)
                    {
                        walkCounter = 0;
                        if (++Projectile.frame >= 10)
                            Projectile.frame = 3;
                    }
                }

                else
                {
                    Projectile.frame = 0;
                }
            }

            Player player = Main.player[Projectile.owner];

            // Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.DuckPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}