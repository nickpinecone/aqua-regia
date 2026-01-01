using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Library.Extended.Modules.Items;

public class ProgressModule : IModule, IItemRuntime
{
    private Tween.Tween<int> _timer = null!;

    public bool Done => _timer.Done;
    public int Duration => _timer.Duration;
    public int Current => _timer.Current;

    public Color? ColorA { get; set; }
    public Color? ColorB { get; set; }
    public Color? ColorBorder { get; set; }
    public Color? ColorBorderActive { get; set; }

    public void SetDefaults(int time)
    {
        _timer = Tween.Tween.Create<int>(time);
    }

    public void SetColors(Color colorA, Color colorB, Color colorBorder, Color colorBorderActive)
    {
        ColorA = colorA;
        ColorB = colorB;
        ColorBorder = colorBorder;
        ColorBorderActive = colorBorderActive;
    }

    public void Update()
    {
        _timer.Delay();
    }

    public void Restart()
    {
        _timer.Restart();
    }

    public void RuntimeHoldItem(BaseItem item, Player player)
    {
        Update();
    }
}