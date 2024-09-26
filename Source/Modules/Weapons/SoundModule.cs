using Terraria.Audio;
using AquaRegia.Utils;

namespace AquaRegia.Modules.Weapons;

public class SoundModule : BaseGunModule
{
    public SoundStyle SoundStyle { get; private set; }
    public float Pitch { get; set; }
    public float PitchVariance { get; set; }

    public SoundModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void SetWater(BaseGun baseGun, float pitch = -0.1f, float pitchVariance = 0.1f)
    {
        SoundStyle = new SoundStyle(AudioPath.Use + "WaterShoot");
        Pitch = pitch;
        PitchVariance = pitchVariance;

        baseGun.Item.UseSound = SoundStyle with {
            Pitch = Pitch,
            PitchVariance = PitchVariance,
        };
    }
}
