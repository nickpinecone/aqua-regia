using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace WaterGuns.Modules.Projectiles;

public enum Ease
{
    Linear,
    InOut,
}

public abstract class BaseAnimation
{
    public bool Finished { get; protected set; } = false;
}

public class Animation<T> : BaseAnimation
    where T : struct
{
    private Dictionary<string, BaseAnimation> _states;

    public string[] Depends { get; set; }
    public Ease Ease { get; set; }
    public T Start { get; set; }
    public T End { get; set; }
    public int Frames { get; set; }

    public int CurrentFrame { get; private set; } = 0;
    public T? Value { get; private set; } = null;

    public Animation(Dictionary<string, BaseAnimation> states)
    {
        _states = states;
    }

    public void Reset()
    {
        Finished = false;
        CurrentFrame = 0;
    }

    public bool CanAnimate()
    {
        return !Depends.Any((depend) => !_states.ContainsKey(depend) || !_states[depend].Finished);
    }

    public T? Update()
    {
        if (Finished || !CanAnimate())
            return null;

        CurrentFrame += 1;

        if (CurrentFrame > Frames)
        {
            Finished = true;
            Value = null;
            return null;
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

        return Value;
    }
}

public class AnimationModule : BaseProjectileModule
{
    private Dictionary<string, BaseAnimation> _states = new();

    public AnimationModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    public Animation<T> Get<T>(string name)
        where T : struct
    {
        if (_states.ContainsKey(name))
        {
            return _states[name] as Animation<T>;
        }
        else
        {
            return null;
        }
    }

    public Animation<T> Animate<T>(string name, T start, T end, int frames, Ease ease, string[] depends = null)
        where T : struct
    {
        depends ??= new string[] { };

        if (!_states.ContainsKey(name))
        {
            _states[name] = new Animation<T>(_states)
            {
                Start = start,
                End = end,
                Frames = frames,
                Ease = ease,
                Depends = depends,
            };
        }

        return _states[name] as Animation<T>;
    }

    public void Sprite(BaseProjectile baseProjectile, int delay)
    {
        baseProjectile.Projectile.frameCounter += 1;

        if (baseProjectile.Projectile.frameCounter >= delay)
        {
            baseProjectile.Projectile.frameCounter = 0;
            baseProjectile.Projectile.frame += 1;

            if (baseProjectile.Projectile.frame >= Main.projFrames[baseProjectile.Projectile.type])
            {
                baseProjectile.Projectile.frame = 0;
            }
        }
    }
}
