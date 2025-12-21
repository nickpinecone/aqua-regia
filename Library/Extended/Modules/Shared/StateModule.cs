using System;
using System.Collections.Generic;

namespace AquaRegia.Library.Extended.Modules.Shared;

public class StateModule<T> : IModule, IProjectileRuntime
    where T : Enum
{
    private readonly Dictionary<T, Action> _handlers = new();

    public T Current { get; set; }

    public StateModule(T current)
    {
        Current = current;
    }

    public void UpdateState()
    {
        if (_handlers.TryGetValue(Current, out var handler))
        {
            handler();
        }
    }

    public void AddState(T state, Action handler)
    {
        _handlers[state] = handler;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        UpdateState();
    }
}