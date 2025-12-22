using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Helpers;

public static class NpcHelper
{
    public static NPC? FindNearestNPC(Vector2 position, float radius, Func<NPC, bool>? canHome = null)
    {
        canHome ??= (_) => true;

        var nearestDistance = float.PositiveInfinity;
        NPC? nearestNpc = null;
        var detectRange = MathF.Pow(radius, 2);

        foreach (var target in Main.npc)
        {
            if (!target.CanBeChasedBy() || !canHome(target))
            {
                continue;
            }

            var distance = position.DistanceSQ(target.Center);

            if (distance <= detectRange && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNpc = target;
            }
        }

        return nearestNpc;
    }
}