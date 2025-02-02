using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BoomerangModule : IModule
{
    public float MaxDistance { get; set; }
    public float PlayerClose { get; set; }
    public Vector2? SpawnPosition { get; set; }

    public bool DidReachFar { get; private set; }
    public bool DidReturn { get; private set; }

    public void SetDefaults(float maxDistance = 500f, float playerClose = 16f)
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

    public bool CheckFar(Vector2 position)
    {
        if (!DidReachFar)
        {
            if (IsFar(position))
            {
                DidReachFar = true;
            }
        }

        return DidReachFar;
    }

    public bool IsClose(Vector2 returnTo, Vector2 position)
    {
        return returnTo.DistanceSQ(position) < PlayerClose * PlayerClose;
    }

    public bool CheckReturn(Vector2 returnTo, Vector2 position)
    {
        if (!DidReturn)
        {
            if (IsClose(returnTo, position))
            {
                DidReturn = true;
            }
        }

        return DidReturn;
    }
}
