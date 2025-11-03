using AquaRegia.Library.Tween.Tweens;
using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Tween;

public static class TweenManager
{
    public static Tween<T> Create<T>(int duration, bool paused = false)
    {
        if (typeof(T) == typeof(float))
        {
            return (new FloatTween(duration, paused) as Tween<T>)!;
        }

        if (typeof(T) == typeof(Vector2))
        {
            return (new Vector2Tween(duration, paused) as Tween<T>)!;
        }

        if (typeof(T) == typeof(IntTween))
        {
            return (new IntTween(duration, paused) as Tween<T>)!;
        }

        return (new DynamicTween<T>(duration, paused));
    }
}