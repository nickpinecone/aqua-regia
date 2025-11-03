namespace AquaRegia.Library.Tween.Tweens;

public class IntTween : Tween<int>
{
    public IntTween(int duration, bool paused = false) : base(duration, paused)
    {
    }

    protected override int Transition(int start, int end, float percent)
    {
        return (int)(start + (end - start) * percent);
    }
}