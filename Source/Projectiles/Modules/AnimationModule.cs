using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.Modules;

// TODO consider using animation classes and creating them

public class AnimationState
{
    public int Frame = 0;
    public bool Finished = false;
}

public enum Easing
{
    Linear,
    InOut,
}

public class AnimationModule : BaseProjectileModule
{
    private Dictionary<string, AnimationState> _states = new();

    public AnimationModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    private bool CanAnimate(string name, int frames, string[] depends)
    {
        if (depends.Any((depend) => !_states.ContainsKey(depend) || !_states[depend].Finished))
        {
            return false;
        }

        if (!_states.ContainsKey(name))
        {
            _states[name] = new AnimationState();
        }
        else if (_states[name].Finished)
        {
            return false;
        }

        _states[name].Frame += 1;

        if (_states[name].Frame >= frames)
        {
            _states[name].Finished = true;
        }

        return true;
    }

    public void ResetAnimation(string name)
    {
        _states[name].Frame = 0;
        _states[name].Finished = false;
    }

    public bool IsFinished(string name)
    {
        if (!_states.ContainsKey(name))
            return false;

        return _states[name].Finished;
    }

    public float? AnimateF(string name, float start, float end, int frames, string[] depends, Easing easing)
    {
        if (!CanAnimate(name, frames, depends))
        {
            return null;
        }
        else
        {
            if (easing == Easing.InOut)
            {
                var t = _states[name].Frame / (float)frames;

                var result = t * t * (3.0f - 2.0f * t);

                return start + (end - start) * result;
            }
            else if(easing == Easing.Linear)
            {
                return start + (end - start) * (_states[name].Frame / (float)frames);
            }
            else
            {
                return null;
            }
        }
    }

    public Vector2? AnimateVec(string name, Vector2 start, Vector2 end, int frames, string[] depends, Easing easing)
    {
        if (!CanAnimate(name, frames, depends))
        {
            return null;
        }
        else
        {
            if (easing == Easing.InOut)
            {
                var t = _states[name].Frame / (float)frames;

                var result = t * t * (3.0f - 2.0f * t);

                return start + (end - start) * result;
            }
            else if(easing == Easing.Linear)
            {
                return start + (end - start) * (_states[name].Frame / (float)frames);
            }
            else
            {
                return null;
            }
        }
    }
}
