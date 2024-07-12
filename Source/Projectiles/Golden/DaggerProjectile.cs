using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Players;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Golden;

public class DaggerProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Projectiles + "DaggerProjectile";

    public AnimationModule Animation { get; private set; }
    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }
    public StickModule Stick { get; private set; }

    public DaggerProjectile() : base()
    {
        Animation = new AnimationModule(this);
        Property = new PropertyModule(this);
        Home = new HomeModule(this);
        Stick = new StickModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 600);

        Home.SetDefaults();
        Home.Speed = 8;

        Projectile.tileCollide = false;
        Projectile.damage = 1;
        Projectile.penetrate = 3;

        Projectile.width = 10;
        Projectile.height = 24;
        Projectile.alpha = 255;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (target.getRect().Intersects(Projectile.getRect()) && Stick.Target == null)
        {
            Stick.ToTarget(target, Projectile.Center);
        }

        return base.CanHitNPC(target);
    }

    public override void AI()
    {
        base.AI();

        var appear = Animation.Animate<int>("appear", 255, 0, 10, Ease.Linear);
        Projectile.alpha = appear.Update() ?? Projectile.alpha;

        if (appear.Finished)
        {
            if (Stick.Target == null)
            {
                Projectile.velocity = Home.Default(Projectile.Center, Projectile.velocity) ?? Projectile.velocity;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else if (Stick.Target != null)
            {
                Projectile.Center = Stick.HitPoint;
            }
        }
    }
}
