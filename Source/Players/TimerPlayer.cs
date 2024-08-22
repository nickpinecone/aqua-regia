using System.Collections.Generic;
using AquaRegia.Utils;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public static class TimerManager
{
    private static List<Timer> _timers = new();
    private static List<Timer> _removeQueue = new();

    public static void Add(Timer timer)
    {
        _timers.Add(timer);
        timer.OnManager = true;
        timer.Managed = true;
    }

    public static void Update()
    {
        foreach (var timer in _timers)
        {
            timer.Update();

            if (timer.Done)
            {
                _removeQueue.Add(timer);
            }
        }

        foreach (var timer in _removeQueue)
        {
            _timers.Remove(timer);
            timer.OnManager = false;
        }
        _removeQueue.Clear();
    }
}

public class TimerPlayer : ModPlayer
{
    public override void PreUpdate()
    {
        base.PreUpdate();

        TimerManager.Update();
    }
}
