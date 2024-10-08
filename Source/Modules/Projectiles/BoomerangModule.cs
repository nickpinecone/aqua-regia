using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class BoomerangModule : BaseProjectileModule
{
    public Animation<Vector2> Slow { get; private set; }
    public HomeModule Home { get; private set; }

    public float MaxDistance { get; set; }
    public Vector2 SpawnPosition { get; set; }
    public float PlayerClose { get; set; } = 16f;

    public bool IsFarAway { get; private set; }
    public bool Returned { get; private set; }

    public BoomerangModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void SetDefaults(HomeModule home, Animation<Vector2> slow, float maxDistance = 0f,
                            Vector2? spawnPosition = null, float playerClose = 16f)
    {
        Home = home;
        Slow = slow;

        MaxDistance = maxDistance;
        SpawnPosition = spawnPosition ?? Vector2.Zero;
        PlayerClose = playerClose;
    }

    public bool Update(Vector2 position)
    {
        if (!IsFarAway && position.DistanceSQ(SpawnPosition) > MaxDistance * MaxDistance)
        {
            IsFarAway = true;
        }

        return IsFarAway;
    }

    public Vector2 ReturnToPlayer(Player player, Vector2 position, Vector2 velocity)
    {
        velocity = Slow.Animate(velocity, Vector2.Zero) ?? velocity;

        if (Slow.Finished)
        {
            velocity = Home.Calculate(position, velocity, player.Center);

            if (player.Center.DistanceSQ(position) < PlayerClose * PlayerClose)
            {
                Returned = true;
            }
        }

        return velocity;
    }
}
