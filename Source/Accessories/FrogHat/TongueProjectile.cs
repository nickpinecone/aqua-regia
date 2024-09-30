using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Accessories.FrogHat;

public class TongueProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Accessories + "FrogHat/TongueProjectile";

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

        Property.SetDefaults(this, 12, 12, 1, -1);
        Property.SetTimeLeft(this, 120);

        Chain.SetTexture(TexturesPath.Accessories + "FrogHat/TongueChain", new Rectangle(0, 0, 6, 16));
        Chain.SetDefaults(512f, 0, null, 32f);
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

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        base.OnTileCollide(oldVelocity);

        _didHit = true;

        return false;
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
