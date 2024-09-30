
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Armor.Diver;

public class FloatBubble : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Sea/BubbleProjectile";

    public PropertyModule Property { get; private set; }

    public FloatBubble()
    {
        Property = new PropertyModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 20, 20, 0, -1, 0, 155, 0, false);
        Property.SetTimeLeft(this, 2);

        Projectile.hide = true;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Particle.Circle(DustID.BubbleBurst_Blue, Projectile.Center, new Vector2(8, 8), 4, 2f, 0.8f);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.scale = 3;
    }

    public override void AI()
    {
        base.AI();

        Projectile.timeLeft = 2;
        Projectile.Center = Helper.ToVector2I(Main.LocalPlayer.Center);
    }

    public override void DrawBehind(int index, System.Collections.Generic.List<int> behindNPCsAndTiles,
                                    System.Collections.Generic.List<int> behindNPCs,
                                    System.Collections.Generic.List<int> behindProjectiles,
                                    System.Collections.Generic.List<int> overPlayers,
                                    System.Collections.Generic.List<int> overWiresUI)
    {
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);

        overPlayers.Add(index);
    }
}
