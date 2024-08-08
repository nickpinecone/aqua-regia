using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Utils;

namespace WaterGuns.Weapons.Granite;

public class GraniteChunk : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteChunk";

    public PropertyModule Property { get; private set; }
    public AnimationModule Animation { get; private set; }

    private Vector2 _endPosition;

    public GraniteChunk() : base()
    {
        Property = new PropertyModule(this);
        Animation = new AnimationModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetDefaults(this);
        Property.SetTimeLeft(this, 35);

        Projectile.damage = 1;
        Projectile.penetrate = -1;

        Projectile.width = 26;
        Projectile.height = 48;
        Projectile.tileCollide = false;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        var graniteSource = (GraniteSource)source;

        var x = graniteSource.Target.Center.X + Main.rand.Next(-8, 8);
        var tiles = TileHelper.FromTop(new Vector2(x, graniteSource.Target.Center.Y), 64);
        tiles = tiles.Where((tile) => TileHelper.IsSolid(Main.tile[tile.X, tile.Y]));

        if (tiles.Count() > 0)
        {
            var tile = tiles.ElementAt(0);
            Projectile.Bottom = tile.ToWorldCoordinates() + new Vector2(Main.rand.Next(-8, 8), 0);

            var direction = Projectile.Center - graniteSource.Target.Center;
            Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;

            _endPosition = Projectile.Center;
            Projectile.Center += (new Vector2(48, 0)).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
        }
        else
        {
            Projectile.Kill();
        }
    }

    public override void AI()
    {
        base.AI();

        var pos = Animation.Animate<Vector2>("pos", Projectile.Center, _endPosition, 5, Ease.InOut);
        Projectile.Center = pos.Update() ?? Projectile.Center;
    }
}
