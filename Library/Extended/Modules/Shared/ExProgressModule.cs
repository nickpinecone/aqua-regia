using AquaRegia.Library.Extended.UI;
using AquaRegia.Library.Tween;
using AquaRegia.UI;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;

namespace AquaRegia.Library.Extended.Modules.Shared;

public class ExProgressModule : IModule
{
    private readonly Tween<int> _timer;
    private readonly FillBox _box;

    public bool Done => _timer.Done;
    public int Duration => _timer.Duration;
    public int Current => _timer.Current;

    public ExProgressModule(Tween<int> timer, int start, Color colorA, Color colorB, Color colorBorder, string tooltip)
    {
        _timer = timer;

        _box = new FillBox(
            StyleDimension.FromPixels(18), StyleDimension.FromPixels(90),
            timer.Duration, 2, tooltip, start,
            colorA, colorB, colorBorder
        );

        ModContent.GetInstance<AquaInterface>().State.BoxContainer.AddElement(_box);
    }

    public void Update(int amount = 1)
    {
        _timer.Delay(amount);
        _box.Current = _timer.Current;
    }

    public void Destroy()
    {
        ModContent.GetInstance<AquaInterface>().State.BoxContainer.RemoveElement(_box);
    }
}