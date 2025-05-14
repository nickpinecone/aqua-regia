namespace WaterGuns.Library.Timer;

public class Timer
{
    public bool Done { get; private set; }
    public bool Paused { get; set; }

    public int Current { get; private set; }
    public int WaitTime { get; set; }

    public int TimeLeft => WaitTime - Current;

    public Timer(int waitTime, bool paused = false)
    {
        WaitTime = waitTime;
        Paused = paused;
    }

    public void Restart()
    {
        Done = false;
        Paused = false;
        Current = 0;
    }

    public Timer Update()
    {
        if (Paused || Done) return this;
        
        Current += 1;

        if (Current >= WaitTime)
        {
            Done = true;
        }

        return this;
    }
}