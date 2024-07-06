using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Projectiles.Modules;
using WaterGuns.Utils;

namespace WaterGuns.Projectiles.Sea;

public class SeaProjectile : BaseProjectile
{
    public override string Texture => TexturesPath.Empty;

    public PropertyModule Property { get; private set; }
    public WaterModule Water { get; private set; }

    public SeaProjectile() : base()
    {
        Property = new PropertyModule(this);
        Water = new WaterModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Water.SetDefaults();
        Property.SetDefaults(this);
        Property.SetDefaultGravity();
        Property.SetTimeLeft(this, 35);

        Projectile.damage = 1;
        Projectile.penetrate = 1;

        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        Water.KillEffect(Projectile.Center, Projectile.velocity);
    }

    public override void OnHitNPC(Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Vector2 position;

        while(true)
        {
            position = target.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 96f;

            var tilePosition = position.ToTileCoordinates();
            var tile = Main.tile[tilePosition.X, tilePosition.Y];
            var isSolid = tile.HasTile && Main.tileSolid[tile.TileType];

            if(!isSolid)
            {
                break;
            }
        }

        SpawnProjectile<BubbleProjectile>(position, Vector2.Zero, 1, 0);
    }

    public override void AI()
    {
        base.AI();

        Projectile.velocity = Property.ApplyGravity(Projectile.velocity);
        Water.CreateDust(Projectile.Center, Projectile.velocity);
    }
}
