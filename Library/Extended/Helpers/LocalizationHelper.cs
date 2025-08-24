using Terraria.Localization;

namespace AquaRegia.Library.Helpers;

public static class LocalizationHelper
{
    public static string GetKey(string suffix) => $"Mods.AquaRegia.{suffix}";

    public static LocalizedText GetLocalization(string suffix) => Language.GetOrRegister(GetKey(suffix));
}