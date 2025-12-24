using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Items;

public class AccuracyModule : IModule, IItemRuntime
{
    public float Inaccuracy { get; set; }

    public void SetInaccuracy(float value)
    {
        Inaccuracy = value;
    }

    public Vector2 ApplyInaccuracy(Vector2 velocity)
    {
        return velocity.RotatedByRandom(MathHelper.ToRadians(Inaccuracy));
    }

    public void RuntimeModifyShootStats(BaseItem item, Player player, ref Vector2 position, ref Vector2 velocity,
        ref int type,
        ref int damage, ref float knockback)
    {
        velocity = ApplyInaccuracy(velocity);
    }
}