namespace AquaRegia.Library.Tween.Complex;

public class PingPongTween<T> : Tween<T>
{
    private readonly Tween<T> _tween;

    public PingPongTween(int duration, bool paused = false) : base(duration, paused)
    {
        _tween = Tween.Create<T>(duration, paused);
    }

    public override Tween<T> Delay(int amount = 1)
    {
        base.Delay(amount);

        if (Done)
        {
            (Start, End) = (End, Start);
            Restart();
        }

        return this;
    }

    public override T Transition(T start, T end, float percent)
    {
        return _tween.Transition(start, end, percent);
    }
}