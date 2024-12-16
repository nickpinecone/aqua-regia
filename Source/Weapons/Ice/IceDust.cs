using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Ice;

public class IceDust : ModDust
{
    public override string Texture => TexturesPath.Weapons + "Ice/IceDust";

    public override void OnSpawn(Dust dust)
    {
        dust.noGravity = true;
    }

    public override Color? GetAlpha(Dust dust, Color lightColor)
    {
        return dust.color * dust.scale * 0.6f;
    }

    public override bool Update(Dust dust)
    {
        float light = 0.1f * dust.scale;
        Lighting.AddLight(dust.position, dust.color.ToVector3() * light);

        return false;
    }
}
