using System.Linq;

namespace AquaRegia.Utils;

public enum Ease
{
    Linear,
    InOut,
}

public class BaseAnimation
{
    public bool Finished { get; protected set; } = false;
}

public class Animation<T> : BaseAnimation
    where T : struct
{
    public BaseAnimation[] Depends { get; set; }
    public Ease Ease { get; set; }
    public T Start { get; set; }
    public T End { get; set; }
    public int Frames { get; set; }

    public int CurrentFrame { get; private set; } = 0;
    public T? Value { get; private set; } = null;
    public bool Initiate { get; set; } = true;

    public Animation(int frames = 0, Ease ease = Ease.Linear, BaseAnimation[] depends = null) : base()
    {
        Frames = frames;
        Ease = ease;
        Depends = depends ?? new BaseAnimation[] { };
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
        if (Ease == Ease.Linear)
        {
            Value = (T)(Start + ((dynamic)End - Start) * (CurrentFrame / (float)Frames));
        }
        else if (Ease == Ease.InOut)
        {
            var percent = CurrentFrame / (float)Frames;
            var bezier = percent * percent * (3.0f - 2.0f * percent);

            Value = (T)(Start + ((dynamic)End - Start) * bezier);
        }

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
            Finished = true;
            return null;
        }

        return Calculate(Start, End);
    }

    public T? Backwards()
    {
        if (CurrentFrame < 0 || !CanAnimate())
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
