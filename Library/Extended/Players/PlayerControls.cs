using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Players;

public class PlayerControls : ModPlayer
{
    private bool _pressingRight = false;
    public bool MouseJustRight { get; private set; }

    private bool _pressingLeft = false;
    public bool MouseJustLeft { get; private set; }

    public override void PreUpdate()
    {
        base.PreUpdate();

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