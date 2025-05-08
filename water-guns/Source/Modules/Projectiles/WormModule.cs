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

    public bool? Colliding(Rectangle projectileHitbox, Rectangle targetHitbox)
    {
        if (projectileHitbox.Intersects(targetHitbox))
        {
            return true;
        }

        foreach (var body in BodySegments)
        {
            if (body.Projectile.getRect().Intersects(targetHitbox))
            {
                return true;
            }
        }

        if (TailSegment!.Projectile.getRect().Intersects(targetHitbox))
        {
            return true;
        }

        return false;
    }

    public void PostUpdate(Vector2 headCenter, int timeLeft)
    {
        var parentCenter = headCenter;

        for (var i = 0; i < BodySegments.Count; i++)
        {
            var body = BodySegments[i];
            body.Projectile.timeLeft = timeLeft;
            var segmentSpace = SegmentSpace;

            if (i == 0)
            {
                segmentSpace /= 2;
            }

            var diff = (parentCenter - body.Projectile.Center).SafeNormalize(Vector2.Zero);
            var angle = diff.ToRotation();

            body.Projectile.rotation = angle + MathHelper.PiOver2;

            if (body.Projectile.Center.Distance(parentCenter) > segmentSpace)
            {
                body.Projectile.Center = parentCenter - diff * segmentSpace;
            }

            parentCenter = body.Projectile.Center;
        }

        TailSegment!.Projectile.timeLeft = timeLeft;

        var diffTail = (parentCenter - TailSegment.Projectile.Center).SafeNormalize(Vector2.Zero);
        var angleTail = diffTail.ToRotation();

        TailSegment.Projectile.rotation = angleTail + MathHelper.PiOver2;

        if (TailSegment.Projectile.Center.Distance(parentCenter) > SegmentSpace)
        {
            TailSegment.Projectile.Center = parentCenter - diffTail * SegmentSpace;
        }
    }
}