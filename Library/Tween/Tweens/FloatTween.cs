namespace AquaRegia.Library.Tween.Tweens;

public class FloatTween : Tween<float>
{
    public FloatTween(int duration, bool paused = false) : base(duration, paused)
    {
    }

    protected override float Transition(float start, float end, float percent)
    {
        return (start + (end - start) * percent);
    }
}