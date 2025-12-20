using AquaRegia.Library.Tween;
using Microsoft.Xna.Framework;

namespace AquaRegia.Library.Extended.Modules.Items;

public class ProgressModule : IModule
{
    public Tween<int> Timer { get; private set; } = null!;

    public Color? ColorA { get; set; }
    public Color? ColorB { get; set; }
    public Color? ColorBorder { get; set; }
    public Color? ColorBorderActive { get; set; }

    public void SetDefaults(Tween<int> timer)
    {
        Timer = timer;
    }
}