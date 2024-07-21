using Terraria;
using Terraria.ModLoader;
using WaterGuns.Players.Accessories;
using WaterGuns.Utils;

namespace WaterGuns.Accessoires;

public class FrogMinion : ModProjectile
{
    public override string Texture => TexturesPath.Accessories + "WeirdFrogHat";

    public Timer AttackTimer { get; private set; }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.damage = 1;
        Projectile.knockBack = 1;
        Projectile.penetrate = -1;
        Projectile.minion = true;

        Projectile.timeLeft = 5;
        Projectile.width = 18;
        Projectile.height = 14;

        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.hide = true;

        AttackTimer = new Timer(120);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Main.LocalPlayer.GetModPlayer<FrogPlayer>().minion = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<FrogPlayer>().minion = null;
    }

    public override void AI()
    {
        base.AI();

        if (Main.LocalPlayer.GetModPlayer<FrogPlayer>().Active)
        {
            Projectile.timeLeft = 5;
        }
        Main.LocalPlayer.GetModPlayer<FrogPlayer>().Active = false;
        Projectile.Center = Main.LocalPlayer.Top;

        AttackTimer.Update();

        var target = Helper.FindNearsetNPC(Main.LocalPlayer.Top, 512f);

        if (target != null)
        {
            var direction = target.Center - Main.LocalPlayer.Top;
            Projectile.spriteDirection = direction.X > 0 ? 1 : -1;
        }
        else
        {
            Projectile.spriteDirection = Main.LocalPlayer.direction;
        }

        if (AttackTimer.Done)
        {
            if (target != null)
            {
                AttackTimer.Restart();

                var direction = target.Center - Main.LocalPlayer.Top;
                direction.Normalize();
                direction *= 24f;

                Projectile.spriteDirection = direction.X > 0 ? 1 : -1;

                Projectile.NewProjectileDirect(Projectile.GetSource_NaturalSpawn(), Main.LocalPlayer.Top, direction,
                                               ModContent.ProjectileType<TongueProjectile>(), 12, 1f, Main.myPlayer);
            }
        }
    }

    public override void DrawBehind(int index, System.Collections.Generic.List<int> behindNPCsAndTiles, System.Collections.Generic.List<int> behindNPCs, System.Collections.Generic.List<int> behindProjectiles, System.Collections.Generic.List<int> overPlayers, System.Collections.Generic.List<int> overWiresUI)
    {
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);

        overPlayers.Add(index);
    }
}
