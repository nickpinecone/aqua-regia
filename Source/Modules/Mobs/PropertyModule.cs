using Terraria.Audio;
using Terraria.ID;

namespace AquaRegia.Modules.Mobs;

public class PropertyModule : IModule
{
    public void SetProperties(BaseMob mob, int width = 0, int height = 0, int damage = 0, int defense = 0,
                              int lifeMax = 0, float knockBackResist = 0f, bool noGravity = false,
                              SoundStyle? hitSound = null, SoundStyle? deathSound = null)
    {
        mob.NPC.width = width;
        mob.NPC.height = height;
        mob.NPC.damage = damage;
        mob.NPC.defense = defense;
        mob.NPC.lifeMax = lifeMax;
        mob.NPC.knockBackResist = knockBackResist;
        mob.NPC.noGravity = noGravity;
        mob.NPC.HitSound = hitSound ?? SoundID.NPCHit1;
        mob.NPC.DeathSound = deathSound ?? SoundID.NPCDeath1;
    }
}
