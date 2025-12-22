using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Global;

public class TileGlobal : GlobalTile
{
    public delegate void KillTileDelegate(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem);
    public static event KillTileDelegate? KillTileEvent;
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        KillTileEvent?.Invoke(i, j, type, ref fail, ref effectOnly, ref noItem);
    }

    public delegate void DropDelegate(int i, int j, int type);
    public static event DropDelegate? DropEvent;
    public override void Drop(int i, int j, int type)
    {
        DropEvent?.Invoke(i, j, type);
    }

    public delegate bool CanDropDelegate(int i, int j, int type);
    public static event CanDropDelegate? CanDropEvent;
    public override bool CanDrop(int i, int j, int type)
    {
        if (CanDropEvent is not null)
        {
            var result = true;

            foreach (var @delegate in CanDropEvent.GetInvocationList())
            {
                var del = (CanDropDelegate)@delegate;
                result |= del(i, j, type);
            }

            return result;
        }

        return base.CanDrop(i, j, type);
    }

    public override void Unload()
    {
        KillTileEvent = null;
        DropEvent = null;
        CanDropEvent = null;
    }
}