using Terraria;
using Terraria.ModLoader;
using WaterGuns.Projectiles.Shotgun;

namespace WaterGuns.Players.Weapons;

public class ShotPlayer : ModPlayer
{
    public bool IsPulling { get; set; } = false;
    public NPC Target { get; set; }
    public ChainProjectile Chain { get; set; }
}
