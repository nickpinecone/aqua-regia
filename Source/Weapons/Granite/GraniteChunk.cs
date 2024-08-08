using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using WaterGuns.Modules;
using WaterGuns.Modules.Projectiles;
using WaterGuns.Players;
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
        Projectile.knockBack = 0;

        Projectile.width = 26;
        Projectile.height = 48;
        Projectile.tileCollide = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        Projectile.damage = 0;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);

        var graniteSource = (GraniteSource)source;

        var x = graniteSource.Target.Center.X + Main.rand.Next(-4, 4);
        var tiles = TileHelper.FromTop(new Vector2(x, graniteSource.Target.Center.Y), 64);
        tiles = tiles.Where((tile) => TileHelper.IsSolid(Main.tile[tile.X, tile.Y]));

        if (tiles.Count() > 0)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath43);
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 6);

            var tile = tiles.ElementAt(0);
            Projectile.Bottom = tile.ToWorldCoordinates() + new Vector2(Main.rand.Next(-4, 4), 0);

            var direction = Projectile.Center - graniteSource.Target.Center;
            Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;

            var vector = (new Vector2(0, 1)).RotatedBy(Projectile.rotation);
            var start = vector.RotatedBy(-MathHelper.PiOver4);
            var end = vector.RotatedBy(MathHelper.PiOver4);

            Particle.Arc(DustID.Granite, Projectile.Bottom, new Vector2(8, 8), start, end, 8, 3f, 0.8f);

            _endPosition = Projectile.Center;
            Projectile.Center += (new Vector2(48, 0)).RotatedBy(Projectile.rotation - MathHelper.PiOver2);

            graniteSource.Target.velocity = new Vector2(vector.X * 4f, vector.Y * 8f);
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

        if (Projectile.timeLeft <= 10)
        {
            var disappear = Animation.Animate<int>("disappear", Projectile.alpha, 255, 10, Ease.Linear);
            Projectile.alpha = disappear.Update() ?? Projectile.alpha;
        }
    }
}
