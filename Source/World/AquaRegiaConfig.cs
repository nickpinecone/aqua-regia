using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AquaRegia.World;

public class AquaRegiaConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("WorldGeneration")]
    [Label("Coral Reef Generation Enabled (Experimental)")]
    [DefaultValue(false)]
    public bool CoralReefGenEnabled { get; set; }
}
