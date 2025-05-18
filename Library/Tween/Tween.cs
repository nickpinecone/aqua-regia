using System;

namespace AquaRegia.Library.Tween;

public class Tween<T>
{
    public bool Done { get; private set; }
    public bool Paused { get; set; }

    public int Current { get; private set; }
    public int Duration { get; set; }

    public int TimeLeft => Duration - Current;

    public T? Start { get; private set; }
    public T? End { get; private set; }

    public Tween(int duration, bool paused = false)
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
            Start = default;
            End = default;
        }
    }

    public Tween<T> Delay()
    {
        if (Paused || Done) return this;

        Current += 1;

        if (Current >= Duration)
        {
            Done = true;
        }

        return this;
    }

    public Tween<T> Transition(T start, T end, bool capture = false)
    {
        if (Paused) return this;
        
        switch (capture)
        {
            case true when Start is null:
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
    
    public Tween<T> OnTransition(Func<float, float> ease, Action<T> action)
    {
        if (Paused) return this;
        
        ArgumentNullException.ThrowIfNull(Start);
        ArgumentNullException.ThrowIfNull(End);
        
        var percent = Current / (float)Duration;
        
        action(TransitionFactory.Transition<T>(Start, End, ease(percent)));
        
        return this;
    }

    public Tween<T> OnTransition(Action<T> action)
    {
        var ease = Ease.Linear;
        
        return OnTransition(ease, action);
    }

    public void OnDone(Action action)
    {
        if (Paused || !Done) return;

        action.Invoke();
    }
}