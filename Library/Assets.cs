namespace AquaRegia.Library;

public static class Assets
{
    private static string Base => "AquaRegia/Assets/";

    private static string Sprites => Base + "Sprites/";
    public static string Empty => Sprites + "Empty";
    public static string Weapons => Sprites + "Weapons/";
    public static string Ammo => Sprites + "Ammo/";

    public static class Audio
    {
        private static string AudioBase => Base + "Audio/";
        public static string Impact => AudioBase + "Impact/";
        public static string Use => AudioBase + "Use/";
    }
}