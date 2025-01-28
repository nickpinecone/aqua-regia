using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BoomerangModule : IModule
{
    public float MaxDistance { get; set; }
    public Vector2 SpawnPosition { get; set; }
    public float PlayerClose { get; set; }

    public void SetDefaults(HomeModule home, Animation<Vector2> slow, Vector2 spawnPosition, float maxDistance = 500f,
                            float backSpeed = 12f, float playerClose = 16f)
    {
        SpawnPosition = spawnPosition;
        MaxDistance = maxDistance;
        PlayerClose = playerClose;
    }

    public bool IsFar(Vector2 position)
    {
        return position.DistanceSQ(SpawnPosition) > MaxDistance * MaxDistance;
    }

    public bool IsReturned(Vector2 returnTo, Vector2 position)
    {
        return returnTo.DistanceSQ(position) < PlayerClose * PlayerClose;
    }
}
