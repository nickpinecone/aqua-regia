using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.NPCs.Swimmer;

public class PoolNoodle : BaseProjectile
{
    public override string Texture => TexturesPath.NPCs + "Swimmer/PoolNoodle";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }

    private int _swingDirection = 1;
    private bool _animationFinished = false;

    public PoolNoodle()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        _immunity.ImmunityTime = 10;

        Property.SetTimeLeft(this, 2);
        Property.SetDefaults(this);

        Projectile.damage = 1;
        Projectile.knockBack = 4f;
        Projectile.penetrate = -1;

        Projectile.width = 64;
        Projectile.height = 8;

        Projectile.tileCollide = false;
        Projectile.hide = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        var player = Main.LocalPlayer;
        player.itemRotation = MathHelper.PiOver2 * -player.direction;
    }

    public override void AI()
    {
        base.AI();

        if (Main.mouseRight || !_animationFinished)
        {
            _animationFinished = false;

            var player = Main.LocalPlayer;

            Projectile.spriteDirection = player.direction;
            Projectile.Center = new Vector2((int)player.Center.X, (int)player.Center.Y);
            Projectile.Center += (new Vector2(32 * player.direction, 0)).RotatedBy(player.itemRotation);

            Projectile.rotation = player.itemRotation;
            Projectile.rotation -= 0.2f * _swingDirection * player.direction;

            var rot = Animation.Animate<float>("rot", player.itemRotation, -player.itemRotation, 12, Ease.InOut);
            player.itemRotation = rot.Update() ?? player.itemRotation;

            var pause = Animation.Animate<float>("pause", 0f, 1f, 4, Ease.Linear, new string[] { "rot" });
            pause.Update();

            if (pause.Finished)
            {
                _animationFinished = true;

                _swingDirection = -_swingDirection;
                rot.Start = player.itemRotation;
                rot.End = -player.itemRotation;
                rot.Reset();
                pause.Reset();
            }

            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.timeLeft = 2;
            Projectile.velocity = new Vector2(player.direction, 0);
        }
    }

    public override void DrawBehind(int index, System.Collections.Generic.List<int> behindNPCsAndTiles,
                                    System.Collections.Generic.List<int> behindNPCs,
                                    System.Collections.Generic.List<int> behindProjectiles,
                                    System.Collections.Generic.List<int> overPlayers,
                                    System.Collections.Generic.List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }
}
