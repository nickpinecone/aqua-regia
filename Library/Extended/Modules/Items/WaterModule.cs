using Terraria.Audio;

namespace AquaRegia.Library.Extended.Modules.Items;

public class WaterModule : IModule, IItemRuntime
{
    public void SetDefaults(BaseItem baseItem, float pitch = -0.1f, float pitchVariance = 0.1f)
    {
        baseItem.Item.UseSound = new SoundStyle(Assets.Audio.Use.water_shoot)
        {
            Pitch = pitch,
            PitchVariance = pitchVariance,
        };
    }

    public void RuntimeSetDefaults(BaseItem baseItem)
    {
        SetDefaults(baseItem);
    }
}