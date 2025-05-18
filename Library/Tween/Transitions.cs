using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Tween;

internal interface ITweenable<T>
{
    public T Tween(T start, T end, float percent);
}

internal static class TransitionFactory
{
    public static T Transition<T>(T start, T end, float percent)
    {
        var tween = start switch
        {
            Vector2 => (ITweenable<T>)(new VectorTween()),
            int => (ITweenable<T>)(new IntTween()),
            float => (ITweenable<T>)(new FloatTween()),
            _ => new DynamicTween<T>()
        };
        
        return tween.Tween(start, end, percent);
    }
}

internal class IntTween : ITweenable<int>
{
    public int Tween(int start, int end, float percent)
    {
        return (int)(start + (end - start) * percent);
    }
}

internal class FloatTween : ITweenable<float>
{
    public float Tween(float start, float end, float percent)
    {
        return (start + (end - start) * percent);
    }
}

internal class VectorTween : ITweenable<Vector2>
{
    public Vector2 Tween(Vector2 start, Vector2 end, float percent)
    {
        return (start + (end - start) * percent);
    }
}

internal class DynamicTween<T> : ITweenable<T>
{
    public T Tween(T start, T end, float percent)
    {
        return (start + ((dynamic)end! - start) * percent);
    }
}
