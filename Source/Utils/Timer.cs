namespace AquaRegia.Utils;

public class Timer
{
    public bool Paused { get; set; }
    public bool Done { get; private set; }
    public bool Started { get; private set; }

    public int Current { get; private set; }
    public int WaitTime { get; set; }
    public int TimeLeft
    {
        get
        {
            return WaitTime - Current;
        }
    }

    public Timer(int waitTime, bool start = true)
    {
        WaitTime = waitTime;
        Started = start;
    }

    public void Restart()
    {
        Started = true;
        Paused = false;
        Done = false;
        Current = 0;
    }

    public void Update()
    {
        if (Started && !Done && !Paused)
        {
            Current += 1;
            if (Current >= WaitTime)
            {
                Done = true;
            }
        }
    }
}
