using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Shotgun;

public class ShotPlayer : ModPlayer
{
    public bool IsPulling { get; set; } = false;
    public NPC? Target { get; set; }
    public ChainProjectile? Chain { get; set; }
}
