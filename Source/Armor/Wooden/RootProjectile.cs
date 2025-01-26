using Microsoft.Xna.Framework;
using Terraria;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;

namespace AquaRegia.Armor.Wooden;

public class RootProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Armor + "Wooden/RootProjectile";

    public PropertyModule Property { get; set; }
    public Animation<int> Appear { get; private set; }
    public Animation<float> Scale { get; private set; }

    public RootProjectile() : base()
    {
        Property = new PropertyModule(this);
        Appear = new Animation<int>(10);
        Scale = new Animation<float>(60, Ease.InOut);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this, 16, 20, 1, -1, 0, 0, 0, false);
        Property.SetTimeLeft(this, 10);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        var woodenSource = source as WoodenSource;

        var side = MathHelper.PiOver4 * woodenSource.Direction;
        side += Main.rand.NextFloat(-0.1f, 0.1f);
        Projectile.rotation = side;
        Projectile.Center += new Vector2(Main.rand.Next(-2, 2), 0);

        Projectile.spriteDirection = Main.rand.NextFromList(new int[] { 1, -1 });
        Projectile.scale = Main.rand.NextFloat(1f, 1.2f);
        Main.LocalPlayer.GetModPlayer<WoodenPlayer>().Root = this;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Main.LocalPlayer.GetModPlayer<WoodenPlayer>().Root = null;

        var particle = Particle.Single(ParticleID.Wood, Projectile.Center, new Vector2(6, 6), Vector2.Zero);
        particle.noGravity = true;
    }

    public override void AI()
    {
        base.AI();

        Projectile.alpha = Appear.Animate(255, 0) ?? Projectile.alpha;

        if (Main.LocalPlayer.velocity.Length() >= 1e-3)
        {
            Projectile.Kill();
        }
        else
        {
            Projectile.timeLeft = 10;
            Projectile.scale = Scale.Loop(Projectile.scale, Projectile.scale + 0.1f);
        }
    }
}
