using System;

namespace AquaRegia.Library.Tween;

public abstract class Tween<T>
{
    public bool Done { get; private set; }
    public bool Paused { get; set; }

    public int Current { get; private set; }
    public int Duration { get; set; }

    public int TimeLeft => Duration - Current;

    private bool _isCaptured = false;
    public T? Start { get; protected set; }
    public T? End { get; protected set; }

    protected Tween(int duration, bool paused = false)
    {
        Duration = duration;
        Paused = paused;
    }

    public void Restart(bool resetValues = false)
    {
        Done = false;
        Paused = false;
        Current = 0;

        if (resetValues)
        {
            _isCaptured = false;
            Start = default;
            End = default;
        }
    }

    public virtual Tween<T> Delay(int amount = 1)
    {
        if (Paused || Done) return this;

        Current += amount;
        Current = Math.Clamp(Current, 0, Duration);

        if (Current >= Duration)
        {
            Done = true;
        }

        return this;
    }

    public Tween<T> Transition(T start, T end, bool capture = true)
    {
        if (Paused || Done) return this;

        switch (capture)
        {
            case true when !_isCaptured:
                _isCaptured = true;
                Start = start;
                End = end;
                break;
            case true:
                break;
            default:
                Start = start;
                End = end;
                break;
        }

        return Delay();
    }

    public abstract T Transition(T start, T end, float percent);

    public Tween<T> OnTransition(Func<float, float> ease, Action<T> action)
    {
        if (Paused || Done) return this;

        ArgumentNullException.ThrowIfNull(Start);
        ArgumentNullException.ThrowIfNull(End);

        var percent = Current / (float)Duration;

        action(Transition(Start, End, ease(percent)));

        return this;
    }

    public Tween<T> OnTransition(Action<T> action)
    {
        var ease = Ease.Ease.Linear;

        return OnTransition(ease, action);
    }

    public void OnDone(Action action)
    {
        if (Paused || !Done) return;

        action.Invoke();
    }
}