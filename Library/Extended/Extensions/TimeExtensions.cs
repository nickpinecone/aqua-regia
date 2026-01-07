namespace AquaRegia.Library.Extended.Extensions;

public static class TimeExtensions
{
    public static int FromSeconds(this float seconds)
    {
        return (int)(seconds * 60);
    }

    public static int FromSeconds(this int seconds)
    {
        return seconds * 60;
    }
    
    public static float ToSeconds(this int time)
    {
        return (float)time / 60;
    }
    
    public static float ToSeconds(this float time)
    {
        return time / 60;
    }
}