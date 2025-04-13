using System;
using AquaRegia.Source.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Source.Players;

public class SwimPlayer : ModPlayer
{
    public override void UpdateEquips()
    {
        base.UpdateEquips();

        Player.accFlipper = true;
        Player.breathMax = 400;
    }

    public int Counter { get; set; } = 60;
    public int CounterMax { get; set; } = 60;
    public int CounterMaxForSwing = 30;
    public int CounterMaxForIdle = 60;
    public Rectangle CurrentFrame { get; set; }
    public Rectangle CurrentFrameLeg { get; set; }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (Player.wet && !Player.lavaWet && !Player.honeyWet)
        {
            if (Player.velocity == Vector2.Zero)
            {
                drawInfo.rotation = MyRotation;
            }
            else
            {
                drawInfo.rotation = Player.velocity.X * 0.1f;
                MyRotation = drawInfo.rotation;
            }

            Counter += 1;

            if (Counter > CounterMax)
            {
                Counter = 0;

                if (Player.velocity == Vector2.Zero)
                {
                    if (Player.bodyFrame == PlayerFrames.Jump.ToRectangle())
                    {
                        CurrentFrame = PlayerFrames.Idle.ToRectangle();
                        CounterMax = CounterMaxForSwing * 2;
                    }
                    else
                    {
                        CurrentFrame = PlayerFrames.Jump.ToRectangle();
                        CounterMax = CounterMaxForIdle * 3;
                    }

                    CurrentFrameLeg = PlayerFrames.Idle.ToRectangle();
                }
                else
                {
                    if (Player.bodyFrame == PlayerFrames.Jump.ToRectangle())
                    {
                        CurrentFrame = PlayerFrames.Idle.ToRectangle();
                        CounterMax = CounterMaxForIdle;
                    }
                    else
                    {
                        CurrentFrame = PlayerFrames.Jump.ToRectangle();
                        CounterMax = CounterMaxForSwing;
                    }

                    CurrentFrameLeg = CurrentFrame;
                }
            }

            if (!Main.mouseLeft)
            {
                Player.legFrame = CurrentFrameLeg;
                Player.bodyFrame = CurrentFrame;
            }
        }
    }

    public Vector2 MyVelocity { get; set; }
    public float MyRotation { get; set; }

    public override void PreUpdateMovement()
    {
        base.PreUpdateMovement();

        if (Player.wet && !Player.lavaWet && !Player.honeyWet)
        {
            Player.velocity = MyVelocity;
            var strength = 0.2f;

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.W] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                Player.velocity += new Vector2(0, -strength);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.S] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                Player.velocity += new Vector2(0, strength);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.A] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                Player.velocity += new Vector2(-strength, 0);
            }

            if (Main.keyState[Microsoft.Xna.Framework.Input.Keys.D] == Microsoft.Xna.Framework.Input.KeyState.Down)
            {
                Player.velocity += new Vector2(strength, 0);
            }

            var normal = Player.velocity.SafeNormalize(Vector2.Zero);
            var length = Player.velocity.Length();
            length = Math.Min(8, length);

            Player.velocity = normal * length;
            Player.velocity = Player.velocity.MoveTowards(Vector2.Zero, 0.1f);
        }

        MyVelocity = Player.velocity;
    }
}