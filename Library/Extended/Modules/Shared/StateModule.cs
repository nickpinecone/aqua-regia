using System;
using System.Collections.Generic;

namespace AquaRegia.Library.Extended.Modules.Shared;

public class StateModule<T> : IProjectileRuntime
    where T : Enum
{
    public T Current { get; set; }
    private Dictionary<T, Action> Handlers { get; } = new();

    public StateModule(T current)
    {
        Current = current;
    }

    public void UpdateState()
    {
        if (Handlers.TryGetValue(Current, out var handler))
        {
            handler();
        }
        else
        {
            throw new Exception($"Handler for state {Current} was not found");
        }
    }

    public void AddState(T state, Action handler)
    {
        Handlers[state] = handler;
    }

    public void RuntimeAI(BaseProjectile projectile)
    {
        UpdateState();
    }
}