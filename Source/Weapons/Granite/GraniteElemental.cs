using System;
using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Granite;

public class GraniteElemental : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteElemental";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }
    public SpriteModule Sprite { get; private set; }

    public GraniteElemental()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
        Sprite = new SpriteModule(this);
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projFrames[Projectile.type] = 7;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 20);

        Projectile.damage = 1;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;

        Projectile.width = 30;
        Projectile.height = 30;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<GranitePlayer>().Deactivate();
        Main.LocalPlayer.velocity = Projectile.velocity / 2f;
    }

    public override void AI()
    {
        base.AI();

        Main.LocalPlayer.Center = Projectile.Center;
        Projectile.rotation += Sprite.RotateOnMove(Projectile.velocity, 0.2f);
        Animation.Sprite(this, 4);
    }
}
