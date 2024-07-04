using System.Collections.Generic;
using System.Linq;

namespace WaterGuns.Projectiles.Modules;

public enum Ease
{
    Linear,
    InOut,
}

public abstract class BaseAnimation
{
    public bool Finished { get; protected set; } = false;

    public abstract void Update();
}

public class Animation<T> : BaseAnimation
    where T : struct
{
    public Ease Ease { get; set; }
    public T Start { get; set; }
    public T End { get; set; }
    public int Frames { get; set; }

    public int CurrentFrame { get; private set; } = 0;
    public T? Value { get; private set; } = null;

    public void Reset()
    {
        Finished = false;
        CurrentFrame = 0;
    }

    public override void Update()
    {
        if (Finished)
            return;

        CurrentFrame += 1;

        if (CurrentFrame > Frames)
        {
            Finished = true;
            Value = null;
            return;
        }

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
    }
}

public class AnimationModule : BaseProjectileModule
{
    private Dictionary<string, BaseAnimation> _states = new();

    public AnimationModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public Animation<T> Animate<T>(string name, T start, T end, int frames, Ease ease, string[] depends)
        where T : struct
    {
        if (!_states.ContainsKey(name))
        {
            _states[name] = new Animation<T>() {
                Start = start,
                End = end,
                Frames = frames,
                Ease = ease,
            };
        }

        if (!depends.Any((depend) => !_states.ContainsKey(depend) || !_states[depend].Finished))
        {
            _states[name].Update();
        }

        return _states[name] as Animation<T>;
    }
}
