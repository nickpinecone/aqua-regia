using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WaterGuns.Config;

#pragma warning disable 612, 618
public class Configuration : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("Debug")]
    [Label("Enable Debug Info")]
    [DefaultValue(false)]
    public bool DebugInfoEnabled
    {
        get; set;
    }
}
#pragma warning restore 612, 618
