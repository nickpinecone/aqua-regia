using Terraria.Audio;

namespace AquaRegia.Library.Extended.Modules.Items;

public class WaterModule : IModule
{
    public void SetUseSound(BaseItem item, float pitch = -0.1f, float pitchVariance = 0.1f)
    {
        item.Item.UseSound = new SoundStyle(Assets.Audio.Use + "WaterShoot")
        {
            Pitch = pitch,
            PitchVariance = pitchVariance,
        };
    }
}