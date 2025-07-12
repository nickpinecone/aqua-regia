using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AquaRegia.Players;

// TODO custom swim controls, with standard dashing
// needs to be easily modifiable from outside for easy accessory boosts
public class SwimPlayer : ModPlayer
{
    public Vector2 SwimVelocity { get; set; } = Vector2.Zero;

    public override void Load()
    {
        base.Load();
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