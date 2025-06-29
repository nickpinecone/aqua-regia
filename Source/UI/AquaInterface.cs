using AquaRegia.Library.UI;
using Terraria.ModLoader;
using Terraria.UI;

namespace AquaRegia.UI;

[Autoload(Side = ModSide.Client)]
public class AquaInterface : InterfaceSystem
{
    protected override void Initialize(out UIState state, out string afterLayer, out string layerName)
    {
        state = new AquaStateUI();
        afterLayer = "Vanilla: Resource Bars";
        layerName = "AquaRegia: Resource Bars";
    }
}

public class AquaStateUI : UIState
{
    private FillBox _box = null!;

    public override void OnInitialize()
    {
        base.OnInitialize();

        _box = new FillBox(StyleDimension.FromPixels(18), StyleDimension.FromPixels(90), 10, 2, "Box");

        _box.Left.Set(-_box.Width.Pixels - 310, 1f);
        _box.Top.Set(90, 0f);
        
        Append(_box);
    }
}