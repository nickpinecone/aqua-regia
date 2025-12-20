using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Items;

public class AccuracyModule : IModule, IItemRuntime
{
    private float _inaccuracy;

    public void SetInaccuracy(float value)
    {
        _inaccuracy = value;
    }

    public Vector2 ApplyInaccuracy(Vector2 velocity)
    {
        return velocity.RotatedByRandom(MathHelper.ToRadians(_inaccuracy));
    }

    public void RuntimeModifyShootStats(BaseItem baseItem, Player player, ref Vector2 position, ref Vector2 velocity,
        ref int type,
        ref int damage, ref float knockback)
    {
        velocity = ApplyInaccuracy(velocity);
    }
}