using AquaRegia.Library.Tween;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Items;

public class ProgressModule : IModule, IItemRuntime
{
    public Tween<int> Timer { get; private set; } = null!;

    public Color? ColorA { get; set; }
    public Color? ColorB { get; set; }
    public Color? ColorBorder { get; set; }
    public Color? ColorBorderActive { get; set; }

    public void SetTimer(Tween<int> timer)
    {
        Timer = timer;
    }

    public void SetColors(Color colorA, Color colorB, Color colorBorder, Color colorBorderActive)
    {
        ColorA = colorA;
        ColorB = colorB;
        ColorBorder = colorBorder;
        ColorBorderActive = colorBorderActive;
    }

    public void RuntimeHoldItem(BaseItem item, Player player)
    {
        Timer.Delay();
    }
}