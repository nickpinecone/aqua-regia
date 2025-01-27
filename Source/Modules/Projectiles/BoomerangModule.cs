using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BoomerangModule : IModule
{
    public Animation<Vector2> Slow { get; private set; }
    public HomeModule Home { get; private set; }

    public float MaxDistance { get; set; }
    public Vector2 SpawnPosition { get; set; }
    public float PlayerClose { get; set; }

    public BoomerangModule()
    {
        Home = new HomeModule();
        Slow = new Animation<Vector2>();
    }

    public void SetDefaults(HomeModule home, Animation<Vector2> slow, Vector2 spawnPosition, float maxDistance = 500f,
                            float backSpeed = 12f, float playerClose = 16f)
    {
        Home = home;
        Slow = slow;

        SpawnPosition = spawnPosition;
        Home.Speed = backSpeed;
        MaxDistance = maxDistance;
        PlayerClose = playerClose;
    }

    public bool IsFar(Vector2 position)
    {
        return position.DistanceSQ(SpawnPosition) > MaxDistance * MaxDistance;
    }

    public Vector2? CalculateReturn(Vector2 returnTo, Vector2 position, Vector2 velocity)
    {
        velocity = Slow.Animate(velocity, Vector2.Zero) ?? velocity;

        if (Slow.Finished)
        {
            velocity = Home.Calculate(position, velocity, returnTo);

            if (returnTo.DistanceSQ(position) < PlayerClose * PlayerClose)
            {
                return null;
            }
        }

        return velocity;
    }
}
