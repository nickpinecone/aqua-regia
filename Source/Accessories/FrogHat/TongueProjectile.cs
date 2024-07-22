using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Accessoires.FrogHat;

public class TongueProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public ChainModule Chain { get; private set; }

    public bool _didHit = false;

    public TongueProjectile() : base()
    {
        Property = new PropertyModule(this);
        Chain = new ChainModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 120);

        Chain.SetTexture(TexturesPath.Accessories + "FrogHat/TongueProjectile", new Rectangle(0, 0, 6, 16));
        Chain.MaxPosition = 512f;
        Chain.PlayerClose = 32f;

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Chain.SpawnPosition = Projectile.Center;
        Chain.BackSpeed = Projectile.velocity.Length() * 1.5f;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        _didHit = true;
    }

    public override void AI()
    {
        base.AI();

        if (Chain.IsFarAway || _didHit)
        {
            Projectile.velocity = Chain.ReturnToPlayer(Main.LocalPlayer.Top, Projectile.Center, Projectile.velocity, 1);

            if (Chain.Returned)
            {
                Projectile.Kill();
            }
        }
        else
        {
            Chain.Update(Projectile.Center);
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Chain.DrawChain(Main.LocalPlayer.Top, Projectile.Center);

        return base.PreDraw(ref lightColor);
    }
}
