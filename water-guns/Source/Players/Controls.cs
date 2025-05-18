using Terraria;
using Terraria.ModLoader;

namespace WaterGuns.Players;

public class Controls : ModPlayer
{
    private bool _pressingRight = false;
    public bool MouseJustRight { get; private set; }

    private bool _pressingLeft = false;
    public bool MouseJustLeft { get; private set; }

    public override void PreUpdate()
    {
        base.PreUpdate();

        // Right mouse
        if (Main.mouseRight && !MouseJustRight && !_pressingRight)
        {
            MouseJustRight = true;
            _pressingRight = true;
        }
        else
        {
            MouseJustRight = false;
        }

        if (!Main.mouseRight)
        {
            _pressingRight = false;
        }

        // Left mouse
        if (Main.mouseLeft && !MouseJustLeft && !_pressingLeft)
        {
            MouseJustLeft = true;
            _pressingLeft = true;
        }
        else
        {
            MouseJustLeft = false;
        }

        if (!Main.mouseLeft)
        {
            _pressingLeft = false;
        }
    }
}
