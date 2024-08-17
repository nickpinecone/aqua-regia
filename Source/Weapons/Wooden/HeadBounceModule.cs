using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;

namespace AquaRegia.Weapons.Wooden;

public class HeadBounceModule : BaseProjectileModule
{
    private PropertyModule _property;

    public HeadBounceModule(BaseProjectile baseProjectile, PropertyModule property) : base(baseProjectile)
    {
        _property = property;
    }

    public Vector2? BounceOff(NPC target, Vector2 position)
    {
        var side = Main.rand.NextFromList<int>(new int[] { 1, -1 });
        var sideVector = new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(30 * side));
        var bounceVelocity = sideVector.RotatedByRandom(MathHelper.ToRadians(15));

        bounceVelocity.Normalize();
        bounceVelocity *= Main.rand.NextFloat(6f, 8f);

        _property.Gravity /= 1.2f;

        return bounceVelocity;
    }

    public bool CanHit(NPC target, Vector2 position)
    {
        return target.Top.Y >= position.Y;
    }
}
