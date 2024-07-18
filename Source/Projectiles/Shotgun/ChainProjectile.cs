using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Shotgun;

public class ChainProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }
    public StickModule Stick { get; private set; }

    private bool _pulling = false;

    public ChainProjectile() : base()
    {
        Property = new PropertyModule(this);
        Chain = new ChainModule(this);
        Stick = new StickModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);

        Chain.SetTexture(TexturesPath.Projectiles + "ChainProjectile", new Rectangle(0, 0, 6, 14));
        Chain.MaxPosition = 768f;
        Chain.PlayerClose = 32f;

        Projectile.damage = 1;
        Projectile.penetrate = -1;
        Projectile.CritChance = 100;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Chain.SpawnPosition = Projectile.Center;
        Chain.BackSpeed = Projectile.velocity.Length();
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);
        SoundEngine.PlaySound(SoundID.Item1);

        Projectile.velocity = Vector2.Zero;
        Stick.ToTarget(target, Projectile.Center);
        Projectile.friendly = false;
    }

    public override void AI()
    {
        base.AI();

        if (Stick.Target == null || Chain.IsFarAway)
        {
            if (Chain.Update(Projectile.Center))
            {
                Projectile.velocity = Chain.ReturnToPlayer(Main.LocalPlayer, Projectile.Center, Projectile.velocity, 1);
            }

            if (Chain.Returned)
            {
                Projectile.Kill();
            }
        }
        else
        {
            Projectile.Center = Stick.HitPoint;

            Main.LocalPlayer.velocity = Chain.ReturnToPlayer(Main.LocalPlayer, Projectile.Center, Projectile.velocity).RotatedBy(MathHelper.Pi);
            _pulling = true;

            if (Main.LocalPlayer.Center.DistanceSQ(Stick.Target.Center) < 32f * 32f)
            {
                _pulling = false;
                Main.LocalPlayer.velocity = Main.LocalPlayer.velocity.RotatedBy(MathHelper.Pi) / 4f;
                Stick.Detach();
                Projectile.Kill();
            }
        }

        if(_pulling && Main.mouseLeft)
        {
            SoundEngine.PlaySound(SoundID.Item36);
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 4);

            var direction = Main.LocalPlayer.Center - Main.MouseWorld;
            direction.Normalize();
            direction *= 12f;
            Main.LocalPlayer.velocity = direction;
            _pulling = false;

            Stick.Detach();
            Projectile.Kill();
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Chain.DrawChain(Main.LocalPlayer.Center, Projectile.Center);

        return base.PreDraw(ref lightColor);
    }
}
