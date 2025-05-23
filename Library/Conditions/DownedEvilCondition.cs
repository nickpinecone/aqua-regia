using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace AquaRegia.Library.Conditions;

public class DownedEvilCondition : IItemDropRuleCondition
{
    public bool CanDrop(DropAttemptInfo info)
    {
        return Condition.DownedBrainOfCthulhu.IsMet() || Condition.DownedEaterOfWorlds.IsMet();
    }

    public bool CanShowItemDropInUI()
    {
        return true;
    }

    public string GetConditionDescription()
    {
        return "After either Brain of Cthulhu or Eater of Worlds is defeated";
    }
}