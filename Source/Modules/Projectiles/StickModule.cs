using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Modules.Projectiles;

public class StickModule : BaseProjectileModule
{
    private Vector2 _localHit = Vector2.Zero;

    public NPC Target { get; private set; }
    public Vector2 BeforeVelocity { get; set; }

    public Vector2 HitPoint
    {
        get
        {
            return Target.Center - _localHit;
        }
    }

    public StickModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public void Detach()
    {
        Target = null;
        _localHit = Vector2.Zero;
    }

    public void ToTarget(NPC target, Vector2 position)
    {
        if (Target == null)
        {
            Target = target;
            _localHit = target.Center - position;
        }
    }

    public Vector2? Update()
    {
        if (Target == null || Target.GetLifePercent() <= 0f)
        {
            Target = null;
            return null;
        }

        return HitPoint;
    }
}
