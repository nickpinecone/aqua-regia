using System;

namespace WaterGuns.Utils;

public class Timer
{
    public event EventHandler? OnDone;

    public bool Paused { get; set; }
    public bool Done { get; private set; }

    public int Current { get; private set; }
    public int WaitTime { get; set; }
    public int TimeLeft
    {
        get
        {
            return WaitTime - Current;
        }
    }

    public Timer(int waitTime)
    {
        WaitTime = waitTime;
    }

    public void Restart()
    {
        Paused = false;
        Done = false;
        Current = 0;
    }

    public void Update()
    {
        if (!Done && !Paused)
        {
            Current += 1;
            if (Current >= WaitTime)
            {
                Done = true;
                OnDone?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
