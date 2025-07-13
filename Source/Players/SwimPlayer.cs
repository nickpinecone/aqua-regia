using System;
using AquaRegia.Library.Data;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class SwimPlayer : ModPlayer
{
    public Vector2 SwimVelocity { get; set; } = Vector2.Zero;
    public float SwimSpeed { get; set; } = 0.2f;
    public float MaxSwimSpeed { get; set; } = 6f;

    public override void Load()
    {
        base.Load();

        On_Player.KeyHoldDown += On_PlayerOnKeyHoldDown;
    }

    public override void PostUpdateEquips()
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center)) return;

        DetermineOxygenConsumption();
    }

    public override void PreUpdateMovement()
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center))
        {
            SwimVelocity = Player.velocity;
            return;
        }

        ApplySwimVelocity();
    }

    private void DetermineOxygenConsumption()
    {
        var depth = 1f - (Main.LocalPlayer.Center.ToTileCoordinates().Y - UnderwaterSystem.TileSeaLevel) /
            LightingSystem.TileDepth;

        depth = MathHelper.Clamp((float)depth, -0.5f, 1f);

        Player.breathEffectiveness += (float)depth;
    }

    private void ApplySwimVelocity()
    {
        var length = SwimVelocity.Length();
        length = Math.Min(length, MaxSwimSpeed);

        SwimVelocity = SwimVelocity.SafeNormalize(Vector2.Zero) * length;
        SwimVelocity = SwimVelocity.MoveTowards(Vector2.Zero, SwimSpeed / 2);
        Player.velocity = SwimVelocity;
    }

    private static void On_PlayerOnKeyHoldDown(On_Player.orig_KeyHoldDown orig, Player self, int keyDir, int holdTime)
    {
        orig(self, keyDir, holdTime);

        if (!UnderwaterSystem.IsUnderwater(self.Center)) return;

        var swimPlayer = self.GetModPlayer<SwimPlayer>();
        swimPlayer.SwimVelocity += keyDir switch
        {
            CardinalDirections.Down => new Vector2(0, swimPlayer.SwimSpeed),
            CardinalDirections.Up => new Vector2(0, -swimPlayer.SwimSpeed),
            CardinalDirections.Right => new Vector2(swimPlayer.SwimSpeed, 0),
            CardinalDirections.Left => new Vector2(-swimPlayer.SwimSpeed, 0),
            _ => Vector2.Zero
        };
    }
}