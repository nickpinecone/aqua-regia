using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Items;

public class SpriteModule : IModule, IItemRuntime
{
    public Vector2 Offset { get; set; } = Vector2.Zero;
    public Vector2 Shift { get; set; } = Vector2.Zero;
    public Vector2 HoldoutOffset { get; set; } = Vector2.Zero;

    public void SetOffsets(Vector2? offset = null, Vector2? holdoutOffset = null, Vector2? shift = null)
    {
        Offset = offset ?? Vector2.Zero;
        HoldoutOffset = holdoutOffset ?? Vector2.Zero;
        Shift = shift ?? Vector2.Zero;
    }

    public Vector2 ApplyOffset(Vector2 position, Vector2 velocity)
    {
        var normalized = velocity.SafeNormalize(Vector2.Zero);
        var offset = new Vector2(position.X + normalized.X * Offset.X, position.Y + normalized.Y * Offset.Y);

        return offset + Shift;
    }

    public Vector2? RuntimeHoldoutOffset(BaseItem item)
    {
        return HoldoutOffset;
    }

    public void RuntimeModifyShootStats(BaseItem item, Player player, ref Vector2 position, ref Vector2 velocity,
        ref int type,
        ref int damage, ref float knockback)
    {
        position = ApplyOffset(position, velocity);
    }
}