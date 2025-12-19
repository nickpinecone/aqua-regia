using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Players;

public class ScreenShake : ModPlayer
{
    public int Time { get; set; }
    public int Magnitude { get; set; }

    public void Activate(int time, int magnitude)
    {
        Time = time;
        Magnitude = magnitude;
    }

    public override void ModifyScreenPosition()
    {
        Time--;

        if (Time > 0)
        {
            Main.screenPosition += new Vector2(Main.rand.Next(Magnitude * -1, Magnitude + 1),
                Main.rand.Next(Magnitude * -1, Magnitude + 1));
        }
    }
}