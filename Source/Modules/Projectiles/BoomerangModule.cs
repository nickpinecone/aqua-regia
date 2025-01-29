using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BoomerangModule : IModule
{
    public float MaxDistance { get; set; }
    public float PlayerClose { get; set; }
    public Vector2? SpawnPosition { get; set; }

    public void SetDefaults(float maxDistance = 500f, float backSpeed = 12f, float playerClose = 16f)
    {
        MaxDistance = maxDistance;
        PlayerClose = playerClose;
    }

    public void SetSpawn(Vector2 position)
    {
        SpawnPosition = position;
    }

    public bool IsFar(Vector2 position)
    {
        ArgumentNullException.ThrowIfNull(SpawnPosition);

        return position.DistanceSQ((Vector2)SpawnPosition) > MaxDistance * MaxDistance;
    }

    public bool IsReturned(Vector2 returnTo, Vector2 position)
    {
        return returnTo.DistanceSQ(position) < PlayerClose * PlayerClose;
    }
}
