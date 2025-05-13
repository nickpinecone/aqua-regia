using System;

namespace WaterGuns.Library;

public class Timer
{
    public bool Done { get; private set; }
    public bool Paused { get; set; }
    public bool OneShot { get; set; }

    public int Current { get; private set; }
    public int WaitTime { get; set; }

    public int TimeLeft => WaitTime - Current;

    public Timer(int waitTime, bool oneShot = true)
    {
        WaitTime = waitTime;
        OneShot = oneShot;
    }

    public void Restart()
    {
        Done = false;
        Paused = false;
        Current = 0;
    }

    public void Update(Action? onDone = null)
    {
        if (Paused) return;

        if (Current < WaitTime)
        {
            Current += 1;
        }
        else
        {
            onDone?.Invoke();
            Done = true;

            if (!OneShot)
            {
                Restart();
            }
        }
    }
}