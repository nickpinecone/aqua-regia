using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using AquaRegia.Modules;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Players;
using AquaRegia.Utils;

namespace AquaRegia.Weapons.Granite;

public class GraniteChunk : BaseProjectile
{
    public override string Texture => TexturesPath.Weapons + "Granite/GraniteChunk";

    public PropertyModule Property { get; private set; }
    public Animation<Vector2> Position { get; private set; }
    public Animation<int> Disappear { get; private set; }

    private Vector2 _endPosition;

    public GraniteChunk() : base()
    {
        Property = new PropertyModule();

        Composite.AddModule(Property);

        Position = new Animation<Vector2>(5, Ease.InOut);
        Disappear = new Animation<int>(10);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Property.SetProperties(this, 24, 24, 1, -1, 0, 0, 0, false);
        Property.SetTimeLeft(this, 35);

        DrawOriginOffsetY = -28;
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

        var tileSurface = TileHelper.ScanSolidSurface(graniteSource.Target.Center, 2, 2, true).ToList();
        Helper.Shuffle(tileSurface);

        if (tileSurface.Count > 0)
        {
            var tile = tileSurface[0];

            SoundEngine.PlaySound(SoundID.NPCDeath43);
            Main.LocalPlayer.GetModPlayer<ScreenShake>().Activate(6, 6);

            Projectile.Center = tile.ToWorldCoordinates();

            var direction = Projectile.Center - graniteSource.Target.Center;
            Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;

            var vector = (new Vector2(0, 1)).RotatedBy(Projectile.rotation);
            var start = vector.RotatedBy(-MathHelper.PiOver4);
            var end = vector.RotatedBy(MathHelper.PiOver4);

            Particle.Arc(DustID.Granite, Projectile.Bottom, new Vector2(8, 8), start, end, 8, 4f, 0.8f);
            _endPosition = Projectile.Center - (new Vector2(42, 0)).RotatedBy(Projectile.rotation - MathHelper.PiOver2);

            Projectile.spriteDirection = Main.rand.NextFromList(new int[] { 1, -1 });
            Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);

            if (!graniteSource.Target.boss)
            {
                graniteSource.Target.velocity = new Vector2(vector.X * 4f, vector.Y * 8f);
            }
        }
        else
        {
            Projectile.Kill();
        }
    }

    public override void AI()
    {
        base.AI();

        Projectile.Center = Position.Animate(Projectile.Center, _endPosition) ?? Projectile.Center;

        if (Projectile.timeLeft <= 10)
        {
            Projectile.alpha = Disappear.Animate(0, 255) ?? Projectile.alpha;
        }
    }
}
