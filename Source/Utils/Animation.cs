using System;
using System.Linq;

namespace AquaRegia.Utils;

public static class Ease
{
    public static Func<float, float> Linear => (x) => x;

    // Ease In Quad
    public static Func<float, float> In => (x) => x * x;

    // Ease Out Quad
    public static Func<float, float> Out => (x) => 1f - (1f - x) * (1f - x);

    // Ease In And Out Quad
    public static Func<float, float> InOut => (x) => x < 0.5f ? 2f * x * x : 1f - MathF.Pow(-2f * x + 2f, 2f) / 2f;
}

public class BaseAnimation
{
    public bool Finished { get; protected set; } = false;
}

public class Animation<T> : BaseAnimation
    where T : struct
{
    public event EventHandler OnFinished;

    public BaseAnimation[] Depends { get; set; }
    public Func<float, float> Easing { get; set; }
    public T Start { get; set; }
    public T End { get; set; }
    public int Frames { get; set; }

    public int CurrentFrame { get; private set; } = 0;
    public T? Value { get; private set; } = null;
    public bool Initiate { get; set; } = true;

    public Animation(int frames = 0, Func<float, float> ease = null, BaseAnimation[] depends = null) : base()
    {
        ease ??= Ease.Linear;

        Frames = frames;
        Easing = ease;
        Depends = depends ?? new BaseAnimation[] {};
    }

    public void Reset()
    {
        Initiate = true;
        Finished = false;
        CurrentFrame = 0;
    }

    public bool CanAnimate()
    {
        return !Depends.Any((depend) => !depend.Finished);
    }

    public T? Calculate(T start, T end)
    {
        var percent = CurrentFrame / (float)Frames;
        var ease = Easing(percent);

        Value = (T)(Start + ((dynamic)End - Start) * ease);

        return Value;
    }

    public T? Animate(T start, T end)
    {
        if (Initiate)
        {
            Initiate = false;

            Start = start;
            End = end;
        }

        if (Finished || !CanAnimate())
            return null;

        CurrentFrame += 1;

        if (CurrentFrame > Frames)
        {
            CurrentFrame = Frames;
            Finished = true;
            OnFinished?.Invoke(this, null);
            return null;
        }

        return Calculate(Start, End);
    }

    public T? Backwards()
    {
        if (CurrentFrame <= 0 || !CanAnimate())
            return null;

        CurrentFrame -= 1;

        if (CurrentFrame < Frames)
        {
            Finished = false;
        }

        return Calculate(End, Start);
    }

    public T Loop(T start, T end)
    {
        if (Initiate)
        {
            Initiate = false;

            Start = start;
            End = end;
        }

        var value = Animate(Start, End);

        if (value != null)
        {
            return (T)value;
        }
        else
        {
            T temp = Start;
            Start = End;
            End = temp;

            Reset();
            Initiate = false;

            return (T)Animate(Start, End);
        }
    }
}
