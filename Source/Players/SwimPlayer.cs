using System;
using AquaRegia.Library.Extended.Data;
using AquaRegia.Library.Extended.Extensions;
using AquaRegia.Library.Tween;
using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class SwimPlayer : ModPlayer
{
    public const float DefaultSwimSpeed = 0.2f;
    public const float DefaultMaxSwimSpeed = 6f;

    public Vector2 SwimVelocity { get; set; } = Vector2.Zero;
    public float SwimSpeedX { get; set; } = DefaultSwimSpeed;
    public float SwimSpeedY { get; set; } = DefaultSwimSpeed;
    public float MaxSwimSpeed { get; set; } = DefaultMaxSwimSpeed;

    public Tween<int> SwimTimer { get; } = TweenManager.Create<int>(60);
    public PlayerFrames CurrentFrame { get; set; } = PlayerFrames.Idle;

    public override void Load()
    {
        base.Load();

        On_Player.KeyHoldDown += On_PlayerOnKeyHoldDown;
    }

    public override void Unload()
    {
        base.Unload();

        On_Player.KeyHoldDown -= On_PlayerOnKeyHoldDown;

        CanModifyDrawInfoEvent = null;
        PreUpdateMovementEvent = null;
        KeyHoldDownEvent = null;
    }

    public override void PostUpdateEquips()
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center)) return;

        DetermineOxygenConsumption();
    }

    private void DetermineOxygenConsumption()
    {
        var depth = 1f - (Main.LocalPlayer.Center.ToTileCoordinates().Y - UnderwaterSystem.TileSeaLevel) /
            LightingSystem.TileDepth;

        depth = MathHelper.Clamp((float)depth, -0.5f, 1f);

        Player.breathEffectiveness += (float)depth;
    }

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


    public delegate bool CanModifyDrawInfoDelegate(PlayerDrawSet drawInfo);

    public static event CanModifyDrawInfoDelegate? CanModifyDrawInfoEvent;

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (!UnderwaterSystem.IsUnderwater(Player.Center)) return;

        if (CanModifyDrawInfoEvent is not null)
        {
            var result = true;

            foreach (var @delegate in CanModifyDrawInfoEvent.GetInvocationList())
            {
                var del = (CanModifyDrawInfoDelegate)@delegate;
                result &= del(drawInfo);
            }

            if (!result) return;
        }

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
            Player.SetLegFrame(CurrentFrame);
        }

        if (
            Player.GetBodyFrame()
            is not PlayerFrames.Use1
            and not PlayerFrames.Use2
            and not PlayerFrames.Use3
            and not PlayerFrames.Use4
        )
        {
            Player.SetBodyFrame(CurrentFrame);
        }
    }

    private void ApplySwimVelocity()
    {
        var length = SwimVelocity.Length();
        length = Math.Min(length, MaxSwimSpeed);

        SwimVelocity = SwimVelocity.SafeNormalize(Vector2.Zero) * length;
        Player.velocity = SwimVelocity;
        SwimVelocity = SwimVelocity.MoveTowards(Vector2.Zero, (SwimSpeedX + SwimSpeedY) / 4);

        MaxSwimSpeed = DefaultMaxSwimSpeed;
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
            (int)CardinalDirections.Down => new Vector2(0, swimPlayer.SwimSpeedY),
            (int)CardinalDirections.Up => new Vector2(0, -swimPlayer.SwimSpeedY),
            (int)CardinalDirections.Right => new Vector2(swimPlayer.SwimSpeedX, 0),
            (int)CardinalDirections.Left => new Vector2(-swimPlayer.SwimSpeedX, 0),
            _ => Vector2.Zero
        };

        KeyHoldDownEvent?.Invoke(swimPlayer, ref velocity);
        swimPlayer.SwimVelocity += velocity;

        swimPlayer.SwimSpeedX = DefaultSwimSpeed;
        swimPlayer.SwimSpeedY = DefaultSwimSpeed;
    }
}