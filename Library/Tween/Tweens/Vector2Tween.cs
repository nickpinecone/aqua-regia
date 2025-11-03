using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Tween.Tweens;

public class Vector2Tween : Tween<Vector2>
{
    public Vector2Tween(int duration, bool paused = false) : base(duration, paused)
    {
    }

    protected override Vector2 Transition(Vector2 start, Vector2 end, float percent)
    {
        return (start + (end - start) * percent);
    }
}