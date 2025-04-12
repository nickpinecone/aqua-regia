using Terraria.Audio;
using WaterGuns.Utils;

namespace WaterGuns.Modules.Weapons;

public class SoundModule : IModule
{
    public SoundStyle SoundStyle { get; private set; }
    public float Pitch { get; set; }
    public float PitchVariance { get; set; }

    public void SetWater(BaseWeapon weapon, float pitch = -0.1f, float pitchVariance = 0.1f)
    {
        SoundStyle = new SoundStyle(AudioPath.Use + "WaterShoot");
        Pitch = pitch;
        PitchVariance = pitchVariance;

        weapon.Item.UseSound = SoundStyle with
        {
            Pitch = Pitch,
            PitchVariance = PitchVariance,
        };
    }
}
