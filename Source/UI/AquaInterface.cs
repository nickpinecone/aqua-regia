using AquaRegia.Library.Extended.UI;
using Terraria.ModLoader;
using Terraria.UI;

namespace AquaRegia.UI;

[Autoload(Side = ModSide.Client)]
public class AquaInterface : InterfaceSystem
{
    public AquaStateUI State => (AquaStateUI)_state!;

    protected override void Initialize(out UIState state, out string afterLayer, out string layerName)
    {
        state = new AquaStateUI();
        afterLayer = "Vanilla: Resource Bars";
        layerName = "AquaRegia: Resource Bars";
    }
}