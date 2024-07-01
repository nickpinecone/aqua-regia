using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WaterGuns.Projectiles.Modules;

public class AnimationState
{
    public int Frame;
    public bool Finished;
}

public class AnimationModule : BaseProjectileModule
{
    private Dictionary<string, AnimationState> _states = new();

    public AnimationModule(BaseProjectile baseProjectile) : base(baseProjectile)
    {
    }

    private bool CanAnimate(string name, int frames, string[] depends)
    {
        if (depends.Any((depend) => !_states[depend].Finished))
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

    public float? AnimateF(string name, float start, float end, int frames, string[] depends)
    {
        if (!CanAnimate(name, frames, depends))
        {
            return null;
        }
        else
        {
            return start + (end - start) * (_states[name].Frame / (float)frames);
        }
    }

    public Vector2? AnimateVec(string name, Vector2 start, Vector2 end, int frames, string[] depends)
    {
        if (!CanAnimate(name, frames, depends))
        {
            return null;
        }
        else
        {
            return start + (end - start) * (_states[name].Frame / (float)frames);
        }
    }
}
