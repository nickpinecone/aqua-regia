
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Armor.Diver;

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

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 2);

        Projectile.penetrate = -1;
        Projectile.tileCollide = false;

        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.alpha = 155;
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
        Projectile.Center = Main.LocalPlayer.Center;
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
