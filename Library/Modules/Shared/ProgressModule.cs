using System.Diagnostics.CodeAnalysis;
using AquaRegia.Library.Tween;
using AquaRegia.Library.UI;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;

namespace AquaRegia.Library.Modules.Shared;

public class ProgressModule<TState> : IModule
    where TState : UIState, IProgressState
{
    private TState? _state;
    
    public FillBox? Box { get; private set; }
    public Tween<int>? Timer { get; private set; }

    public TState GetState()
    {
        return _state ??= ModContent.GetInstance<TState>();
    }
    
    [MemberNotNull(nameof(Box), nameof(Timer))]
    public void Initialize(FillBox fillBox, Tween<int> timer)
    {
        Box = fillBox;
        Timer = timer;
        GetState().BoxContainer.AddElement(Box);
    }

    [MemberNotNull(nameof(Box), nameof(Timer))]
    public void Initialize(Tween<int> timer)
    {
        Timer = timer;
        Box = GetState().PrimaryBox;
    }

    public void Update()
    {
        if (Box is null || Timer is null) return;
        
        Box.Current = 0;
        Box.ColorBorder = Color.White;

        Timer.Delay().OnTransition((_) =>
        {
            Box.Current = Timer.Current;
            Box.ColorBorder = Timer.Done ? Color.Gold : Color.White;
        });
    }

    public void Remove()
    {
        if (Box is null || Timer is null) return;
        
        GetState().BoxContainer.RemoveElement(Box);
        
        Timer = null;
        Box = null;
    }
}