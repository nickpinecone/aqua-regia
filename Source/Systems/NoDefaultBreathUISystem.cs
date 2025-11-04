using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Systems;

// TODO uncomment when i make a custom ui for this
public class NoDefaultBreathUISystem : ModSystem
{
    public override void Load()
    {
        base.Load();

        // On_Main.DrawInterface_Resources_Breath += On_MainOnDrawInterface_Resources_Breath;
    }

    public override void Unload()
    {
        base.Unload();

        // On_Main.DrawInterface_Resources_Breath -= On_MainOnDrawInterface_Resources_Breath;
    }

    private void On_MainOnDrawInterface_Resources_Breath(On_Main.orig_DrawInterface_Resources_Breath orig)
    {
    }
}