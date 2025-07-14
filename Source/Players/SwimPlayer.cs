using System;
using AquaRegia.Library.Data;
using AquaRegia.Library.Tween;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class SwimPlayer : ModPlayer
{
    public Vector2 SwimVelocity { get; set; } = Vector2.Zero;
    public float SwimSpeed { get; set; } = 0.2f;
    public float MaxSwimSpeed { get; set; } = 6f;

    public Tween<int> SwimTimer { get; } = new Tween<int>(60);
    public int CurrentFrame { get; set; } = PlayerFrames.Idle;

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

    public bool HijackVerticalMovement { get; set; }

    public delegate void PreUpdateMovementDelegate(SwimPlayer player);

    public static event PreUpdateMovementDelegate? PreUpdateMovementEvent;

    public override void PreUpdateMovement()
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center))
        {
            SwimVelocity = Player.velocity;
            return;
        }

        PreUpdateMovementEvent?.Invoke(this);

        ApplySwimVelocity();
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center)) return;

        drawInfo.rotation = MathHelper.Clamp(Player.velocity.X * 0.05f, -0.1f, 0.1f);

        if (SwimTimer.Delay().Done)
        {
            SwimTimer.Restart();

            CurrentFrame = CurrentFrame == PlayerFrames.Idle
                ? PlayerFrames.Jump
                : PlayerFrames.Idle;
        }

        if (SwimVelocity.Length() >= 0.01f)
        {
            Player.legFrame.Y = Player.legFrame.Height * CurrentFrame;
        }

        if (Player.bodyFrame.Y / Player.bodyFrame.Height
            is not PlayerFrames.Use1
            and not PlayerFrames.Use2
            and not PlayerFrames.Use3
            and not PlayerFrames.Use4
           )
        {
            Player.bodyFrame.Y = Player.bodyFrame.Height * CurrentFrame;
        }
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

        // TODO need to think about items that alter these values, need to reset it somewhere
        // ideally at the end or the start of all updates
        // TODO also probably need separate Max Speed for vertical and horizontal
        // too many items specifically for vertical movement
        MaxSwimSpeed = 6f;
    }

    public delegate void KeyHoldDownDelegate(SwimPlayer player, ref Vector2 velocity);
    public static event KeyHoldDownDelegate? KeyHoldDownEvent;
    private static void On_PlayerOnKeyHoldDown(On_Player.orig_KeyHoldDown orig, Player self, int keyDir, int holdTime)
    {
        orig(self, keyDir, holdTime);

        if (!UnderwaterSystem.IsUnderwater(self.Center)) return;
        var swimPlayer = self.GetModPlayer<SwimPlayer>();

        var velocity = keyDir switch
        {
            CardinalDirections.Down => new Vector2(0, swimPlayer.SwimSpeed),
            CardinalDirections.Up => new Vector2(0, -swimPlayer.SwimSpeed),
            CardinalDirections.Right => new Vector2(swimPlayer.SwimSpeed, 0),
            CardinalDirections.Left => new Vector2(-swimPlayer.SwimSpeed, 0),
            _ => Vector2.Zero
        };

        KeyHoldDownEvent?.Invoke(swimPlayer, ref velocity);
        swimPlayer.SwimVelocity += velocity;
    }
}