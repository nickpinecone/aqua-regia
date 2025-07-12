using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Players;

// TODO custom swim controls, with standard dashing
// needs to be easily modifiable from outside for easy accessory boosts
public class SwimPlayer : ModPlayer
{
    public Vector2 SwimVelocity { get; set; } = Vector2.Zero;

    public override void PostUpdateEquips()
    {
        // Consume more oxygen the deeper we go
        if (UnderwaterSystem.IsUnderwater(Main.LocalPlayer.Center))
        {
            var depth = 1f - (Main.LocalPlayer.Center.ToTileCoordinates().Y - UnderwaterSystem.TileSeaLevel) /
                LightingSystem.TileDepth;

            depth = MathHelper.Clamp((float)depth, -0.5f, 1f);

            Main.LocalPlayer.breathEffectiveness += (float)depth;
        }
    }

    public override void PreUpdateMovement()
    {
        base.PreUpdateMovement();

        if (UnderwaterSystem.IsUnderwater(Player.Center))
        {
            // Player.velocity = SwimVelocity;
        }
    }
}