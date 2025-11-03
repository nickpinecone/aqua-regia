using AquaRegia.Library.Extended.Helpers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AquaRegia.Library.Extended.Conditions;

public class DownedEvilCondition : IItemDropRuleCondition
{
    public static LocalizedText Description { get; private set; }

    static DownedEvilCondition()
    {
        Description = LocalizationHelper.GetLocalization($"Conditions.{nameof(DownedEvilCondition)}");
    }

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
        return Description.Value;
    }
}