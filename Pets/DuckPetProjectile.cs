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
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;

            // This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
            // * It uses fluent API syntax, just like Recipe
            // * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
            // * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
            // * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
            // * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
            // ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
            //     .WithOffset(-10, -20f)
            //     .WithSpriteDirection(-1)
            //     .WithCode(DelegateMethods.CharacterPreview.BerniePet);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Penguin); // Copy the stats of the Zephyr Fish

            Projectile.width = 24;
            Projectile.height = 32;

            AIType = ProjectileID.Penguin; // Mimic as the Zephyr Fish during AI.
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.penguin = false;

            return true;
        }

        public override void AI()
        {
            var tilePosition = Projectile.position.ToTileCoordinates();
            var tile = Main.tile[tilePosition.X, tilePosition.Y];

            if (tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Water)
            {
                if (Projectile.velocity.Y > 0)
                {
                    Projectile.velocity.Y = 0;
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