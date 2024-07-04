using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.Modules;

public enum Ease
{
    Linear,
    InOut,
}

public class AnimationF
{
    public Ease Ease { get; set; }
    public float Start { get; set; }
    public float End { get; set; }
    public int Frames { get; set; }

    public int CurrentFrame { get; private set; } = 0;
    public bool Finished { get; private set; } = false;
    public float? Value { get; private set; } = null;

    public void Reset()
    {
        Finished = false;
        CurrentFrame = 0;
    }

    public void Update()
    {
        if (Finished)
            return;

        if (Ease == Ease.Linear)
        {
            Value = Start + (End - Start) * (CurrentFrame / (float)Frames);
        }
        else if (Ease == Ease.InOut)
        {
            var percent = CurrentFrame / (float)Frames;
            var bezier = percent * percent * (3.0f - 2.0f * percent);

            Value = Start + (End - Start) * bezier;
        }

        CurrentFrame += 1;

        if (CurrentFrame >= Frames)
        {
            Finished = true;
            Value = null;
        }
    }
}

public class AnimationVec
{
    public AnimationF X { get; set; }
    public AnimationF Y { get; set; }

    public Vector2 Start
    {
        get
        {
            return new Vector2(X.Start, Y.Start);
        }
        set
        {
            X.Start = value.X;
            Y.Start = value.Y;
        }
    }

    public Vector2 End
    {
        get
        {
            return new Vector2(X.End, Y.End);
        }
        set
        {
            X.End = value.X;
            Y.End = value.Y;
        }
    }

    public int Frames
    {
        get
        {
            return X.Frames;
        }
        set
        {
            X.Frames = value;
            Y.Frames = value;
        }
    }

    public int CurrentFrame
    {
        get
        {
            return X.CurrentFrame;
        }
    }
    public bool Finished
    {
        get
        {
            return X.Finished;
        }
    }
    public Vector2? Value
    {
        get
        {
            if (X.Value is null)
            {
                return null;
            }
            else
            {
                return new Vector2((float)X.Value, (float)Y.Value);
            }
        }
    }

    public Ease Ease
    {
        get
        {
            return X.Ease;
        }
    }

    public void Reset()
    {
        X.Reset();
        Y.Reset();
    }
}

public class AnimationModule : BaseProjectileModule
{
    private Dictionary<string, AnimationF> _states = new();

    public AnimationModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public AnimationF AnimateF(string name, float start, float end, int frames, Ease ease, string[] depends)
    {
        if (!_states.ContainsKey(name))
        {
            _states[name] = new AnimationF()
            {
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

        return _states[name];
    }

    public AnimationVec AnimateVec(string name, Vector2 start, Vector2 end, int frames, Ease ease, string[] depends)
    {
        var x = AnimateF(name + "X", start.X, end.X, frames, ease, depends);
        var y = AnimateF(name + "Y", start.Y, end.Y, frames, ease, depends);

        return new AnimationVec()
        {
            X = x,
            Y = y,
        };
    }
}
