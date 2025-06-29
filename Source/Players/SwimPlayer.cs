using AquaRegia.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Players;

public class SwimPlayer : ModPlayer
{
    public override void Load()
    {
        base.Load();

        On_Player.DoCommonDashHandle += DashHandle;
    }

    private static void DashHandle(On_Player.orig_DoCommonDashHandle orig, Player self, out int dir, out bool dashing,
        Player.DashStartAction dashStartAction)
    {
        orig(self, out dir, out dashing, dashStartAction);

        if (dashing)
        {
            var swimPlayer = self.GetModPlayer<SwimPlayer>();

            swimPlayer.MyVelocity = new Vector2(dir * 14, swimPlayer.MyVelocity.Y);
            self.velocity = swimPlayer.MyVelocity;
        }
    }

    public Vector2 MyVelocity { get; set; }
    public float MyRotation { get; set; }

    // TODO do a complete refactor of this, dashing is a bit too long
    // also need to animate the player swimming
    public override void PreUpdateMovement()
    {
        base.PreUpdateMovement();

        if (UnderwaterSystem.IsUnderwater(Player))
        {
            Player.velocity = MyVelocity;
            var strength = 0.2f;

            var length = Player.velocity.Length();

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.W] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                if (length < 6)
                    Player.velocity += new Vector2(0, -strength);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.S] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                if (length < 6)
                    Player.velocity += new Vector2(0, strength);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.A] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                if (length < 6)
                    Player.velocity += new Vector2(-strength, 0);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.D] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                if (length < 6)
                    Player.velocity += new Vector2(strength, 0);
            }

            length = Player.velocity.Length();

            var normal = Player.velocity.SafeNormalize(Vector2.Zero);
            Player.velocity = normal * length;
            Player.velocity = Player.velocity.MoveTowards(Vector2.Zero, 0.1f);
        }

        MyVelocity = Player.velocity;
    }
}