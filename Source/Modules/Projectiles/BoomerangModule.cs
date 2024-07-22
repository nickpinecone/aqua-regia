using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Modules.Projectiles;

public class BoomerangModule : BaseProjectileModule
{
    public AnimationModule Animation { get; private set; }
    public HomeModule Home { get; private set; }

    public float MaxPosition { get; set; }
    public Vector2 SpawnPosition { get; set; }
    public float PlayerClose { get; set; } = 16f;

    public bool IsFarAway { get; private set; }
    public bool Returned { get; private set; }

    public BoomerangModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
        Animation = new AnimationModule(baseProjectile);
        Home = new HomeModule(baseProjectile);
    }

    public bool Update(Vector2 position)
    {
        if (!IsFarAway && position.DistanceSQ(SpawnPosition) > MaxPosition * MaxPosition)
        {
            IsFarAway = true;
        }

        return IsFarAway;
    }

    public Vector2 ReturnToPlayer(Player player, Vector2 position, Vector2 velocity, int slowFrames = 20)
    {
        var slow = Animation.Animate<Vector2>("slow", velocity, Vector2.Zero, slowFrames, Ease.InOut);
        velocity = slow.Update() ?? velocity;

        if (slow.Finished)
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
