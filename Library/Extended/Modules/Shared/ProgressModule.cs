using AquaRegia.Library.Tween;

namespace AquaRegia.Library.Extended.Modules.Shared;

public class ProgressModule : IModule
{
    public Tween<int> Timer { get; private set; } = null!;
    public bool Done => Timer.Done;

    public void Initialize(Tween<int> timer)
    {
        Timer = timer;
    }

    public void Update()
    {
        Timer.Delay();
    }

    public void Reset()
    {
        Timer.Restart();
    }
}