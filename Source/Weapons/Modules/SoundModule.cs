
using Terraria.Audio;

namespace WaterGuns.Weapons.Modules;

public class SoundModule : BaseGunModule
{
    public SoundStyle SoundStyle { get; private set; }
    public float Pitch { get; set; }
    public float PitchVariance { get; set; }

    public SoundModule(BaseGun baseGun) : base(baseGun)
    {
    }

    public void SetWater(BaseGun baseGun)
    {
        SoundStyle = new SoundStyle("WaterGuns/Assets/Audio/WaterGunShoot");
        Pitch = -0.1f;
        PitchVariance = 0.1f;

        baseGun.Item.UseSound = SoundStyle with
        {
            Pitch = Pitch,
            PitchVariance = PitchVariance,
        };
    }
}
