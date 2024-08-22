using System;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Pets.RubberDuck;

public class RubberDuckProjectile : ModProjectile
{
    public override string Texture => TexturesPath.Pets + "RubberDuck/RubberDuckProjectile";

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 11;
        Main.projPet[Projectile.type] = true;

        ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] =
            ProjectileID.Sets.SimpleLoop(3, 8, 4)
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
        var tilePosition = (Projectile.position + new Vector2(0, 12)).ToTileCoordinates();
        var tilePositionBelow = (Projectile.Bottom + new Vector2(0, -12)).ToTileCoordinates();
        var tile = Main.tile[tilePosition.X, tilePosition.Y];
        var tileBelow = Main.tile[tilePositionBelow.X, tilePositionBelow.Y];

        if (tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Water)
        {
            Projectile.velocity.Y = -8f;
        }
        else if (tileBelow.LiquidAmount > 0 && tileBelow.LiquidType == LiquidID.Water)
        {
            if (Projectile.velocity.Y > 0)
            {
                Projectile.velocity.Y = 0;
            }

            Projectile.frame = 1;
        }
        else
        {
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

        if (!player.dead && player.HasBuff(ModContent.BuffType<RubberDuckBuff>()))
        {
            Projectile.timeLeft = 2;
        }
    }
}
