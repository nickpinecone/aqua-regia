using Microsoft.Xna.Framework;
using Terraria;

namespace WaterGuns.Projectiles.Modules;

public class StickModule : BaseProjectileModule
{
    public NPC Target { get; private set; }
    public Vector2 HitPoint { get; private set; }

    public StickModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void ToTarget(NPC target, Vector2 position)
    {
        if (Target == null)
        {
            Target = target;
            HitPoint = target.Center - position;
        }
    }

    public Vector2? Update()
    {
        if (Target == null || Target.GetLifePercent() <= 0f)
        {
            Target = null;
            return null;
        }

        return Target.Center - HitPoint;
    }
}
