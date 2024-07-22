using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Armor.Wooden;

public class RootProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Armor + "Wooden/RootProjectile";

    public PropertyModule Property { get; set; }
    public AnimationModule Animation { get; set; }

    public RootProjectile() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 10);

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 16;
        Projectile.height = 20;
        Projectile.alpha = 255;

        Projectile.tileCollide = false;
        Projectile.hostile = false;
        Projectile.friendly = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        Projectile.rotation = Main.rand.NextFloat(-0.4f, 0.4f);
        Projectile.scale = Main.rand.NextFloat(1.2f, 1.6f);
        Main.LocalPlayer.GetModPlayer<WoodenPlayer>().Root = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<WoodenPlayer>().Root = null;
    }

    public override void AI()
    {
        base.AI();

        var appear = Animation.Animate<int>("appear", 255, 0, 10, Ease.Linear);
        Projectile.alpha = appear.Update() ?? Projectile.alpha;

        if (Main.LocalPlayer.velocity.Length() >= 1e-3)
        {
            Projectile.Kill();
        }
        else
        {
            Projectile.timeLeft = 10;

            var upscale =
                Animation.Animate<float>("upscale", Projectile.scale, Projectile.scale + 0.1f, 60, Ease.InOut);
            var downscale = Animation.Animate<float>("downscale", Projectile.scale + 0.1f, Projectile.scale, 60,
                                                     Ease.InOut, new string[] { "upscale" });

            Projectile.scale = upscale.Update() ?? Projectile.scale;
            Projectile.scale = downscale.Update() ?? Projectile.scale;

            if(downscale.Finished)
            {
                upscale.Reset();
                downscale.Reset();
            }
        }
    }
}
