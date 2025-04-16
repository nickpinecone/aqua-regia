using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Source.Systems;

public class DebugSystem : ModSystem
{
    public static bool JustPressed(Keys key)
    {
        return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
    }

    public override void PostUpdateWorld()
    {
        if (JustPressed(Keys.D0))
        {
            TestMethod((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        }
    }

    private void TestMethod(int x, int y)
    {
    }
}