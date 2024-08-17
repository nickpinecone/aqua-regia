using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace AquaRegia.Weapons.Space;

public class SpaceShip : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Space/SpaceShip";

    public PropertyModule Property { get; private set; }
    public HomeModule Home { get; private set; }
    public AnimationModule Animation { get; private set; }

    private SpaceLaser _laser = null;

    public SpaceShip() : base()
    {
        Property = new PropertyModule(this);
        Home = new HomeModule(this);
        Animation = new AnimationModule(this);
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projFrames[Projectile.type] = 4;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 240);

        Projectile.penetrate = -1;
        Projectile.width = 38;
        Projectile.height = 32;

        Projectile.tileCollide = false;
        Projectile.hostile = false;
        Projectile.friendly = true;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        _laser = SpawnProjectile<SpaceLaser>(Projectile.Bottom, Vector2.Zero, 1, 1);
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        _laser.Projectile.Kill();
    }

    public override void PostAI()
    {
        if (_laser != null)
        {
            _laser.Projectile.Top = Projectile.Bottom;
        }
    }
}
