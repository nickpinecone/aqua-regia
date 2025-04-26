using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using WaterGuns.Utils;

namespace WaterGuns.Modules.Projectiles;

public class WormModule<TBody, TTail> : IModule
    where TBody : BaseProjectile
    where TTail : BaseProjectile
{
    public List<TBody> BodySegments { get; set; } = [];
    public TTail? TailSegment { get; set; }

    public int SegmentAmount { get; set; }
    public float SegmentSpace { get; set; }

    public void SetDefaults(int segmentAmount = 0, float segmentSpace = 0f)
    {
        SegmentAmount = segmentAmount;
        SegmentSpace = segmentSpace;
    }

    public void SpawnSegments(IEntitySource source, Player player, Vector2 position, int damage, float knockBack)
    {
        for (int i = 0; i < SegmentAmount; i++)
        {
            var body = Helper.SpawnProjectile<TBody>(source, player, position, Vector2.Zero, damage, knockBack);
            BodySegments.Add(body);
        }

        TailSegment = Helper.SpawnProjectile<TTail>(source, player, position, Vector2.Zero, damage, knockBack);
    }

    public void PostUpdate(BaseProjectile head, Vector2 velocity)
    {
        var parent = head;

        foreach (var body in BodySegments)
        {
            if (body.Projectile.Center.Distance(parent.Projectile.Center) > SegmentSpace)
            {
                var position = parent.Projectile.position - (Vector2.UnitY.RotatedBy(parent.Projectile.rotation).RotatedBy(MathHelper.Pi) * SegmentSpace);
                var angle = (parent.Projectile.Center - body.Projectile.Center).ToRotation();

                body.Projectile.rotation = angle + MathHelper.PiOver2;
                body.Projectile.position = position;

                parent = body;
            }
        }

        var positionTail = parent.Projectile.position - (Vector2.UnitY.RotatedBy(parent.Projectile.rotation).RotatedBy(MathHelper.Pi) * SegmentSpace);
        var angleTail = (parent.Projectile.Center - TailSegment!.Projectile.Center).ToRotation();

        TailSegment.Projectile.rotation = angleTail + MathHelper.PiOver2;
        TailSegment.Projectile.position = positionTail;
    }
}