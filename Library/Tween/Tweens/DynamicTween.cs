namespace AquaRegia.Library.Tween.Tweens;

public class DynamicTween<T> : Tween<T>
{
    public DynamicTween(int duration, bool paused = false) : base(duration, paused)
    {
    }

    public override T Transition(T start, T end, float percent)
    {
        return (start + ((dynamic)end! - start) * percent);
    }
}