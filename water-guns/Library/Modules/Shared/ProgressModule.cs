using System.Diagnostics.CodeAnalysis;
using Terraria.ModLoader;
using Terraria.UI;
using WaterGuns.Library.UI;

namespace WaterGuns.Library.Modules.Shared;

public class ProgressModule<TState> : IModule
    where TState : UIState, IHaveHStack
{
    private TState? _state;
    public FillBox? Box;

    public TState GetState()
    {
        return _state ??= ModContent.GetInstance<TState>();
    }
    
    [MemberNotNull(nameof(Box))]
    public void Create(FillBox fillBox)
    {
        Box = fillBox;
        GetState().HStack.AddElement(Box);
    }

    public void Remove()
    {
        if (Box is null) return;
        
        GetState().HStack.RemoveElement(Box);
        Box = null;
    }
}