using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class NoCratesPlayer : ModPlayer
{
    public override void ModifyFishingAttempt(ref FishingAttempt attempt)
    {
        base.ModifyFishingAttempt(ref attempt);
        attempt.crate = false;
    }
}