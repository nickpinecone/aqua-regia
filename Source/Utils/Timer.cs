using WaterGuns.Projectiles;
using WaterGuns.Weapons;

namespace WaterGuns.Utils;

public class Timer
{
    public bool Paused { get; set; }
    public bool Done { get; private set; }
    public int Current { get; private set; }
    public int WaitTime { get; set; }
    public int TimeLeft
    {
        get {
            return WaitTime - Current;
        }
    }

    private Timer(int waitTime)
    {
        WaitTime = waitTime;
    }

    public Timer(int waitTime, BaseGun baseGun) : this(waitTime)
    {
        baseGun.AddTimer(this);
    }

    public Timer(int waitTime, BaseProjectile baseProjectile) : this(waitTime)
    {
        baseProjectile.AddTimer(this);
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
            }
        }
    }
}
