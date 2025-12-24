using AquaRegia.Library.Extended;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Weapons.WoodenWater;

public class HeadBounceModule : IModule
{
    public Vector2 BounceOff(NPC target, Vector2 position)
    {
        var side = Main.rand.NextFromList(1, -1);
        var sideVector = new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(30 * side));
        var bounceVelocity = sideVector.RotatedByRandom(MathHelper.ToRadians(15));

        bounceVelocity = bounceVelocity.SafeNormalize(Vector2.Zero);
        bounceVelocity *= Main.rand.NextFloat(6f, 8f);

        return bounceVelocity;
    }

    public bool CanHit(NPC target, Vector2 position)
    {
        return target.Top.Y >= position.Y;
    }
}