using System;
using System.Collections.Generic;

namespace AquaRegia.Library;

public interface IState<T>
    where T : Enum
{
    protected T CurrentState { get; set; }
    protected Dictionary<T, Action> StateHandlers { get; }

    protected void ConfigureStates();

    protected void UpdateState()
    {
        if (StateHandlers.TryGetValue(CurrentState, out var handler))
        {
            handler();
        }
        else
        {
            throw new Exception($"Handler for state {CurrentState.ToString()} was not found");
        }
    }

    protected void AddState(T state, Action handler)
    {
        StateHandlers[state] = handler;
    }
}