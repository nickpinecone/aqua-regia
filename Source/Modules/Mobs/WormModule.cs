using System;
using AquaRegia.Modules.Projectiles;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AquaRegia.Modules.Mobs;

public enum WormBodyType
{
    Head,
    Body,
    Tail
}

public class WormModule : IModule
{
    public WormBodyType BodyType { get; private set; }
    public Vector2 PastPosition { get; private set; }
    public float PastRotation { get; private set; }
    public WormModule? Parent { get; private set; } = null;

    public int SegmentAmount { get; private set; }
    public float SegmentSpace { get; private set; }

    public void SetDefaults(WormBodyType bodyType, WormModule? parent = null, int segmentAmount = 0, float segmentSpace = 0f)
    {
        BodyType = bodyType;
        Parent = parent;
        SegmentAmount = segmentAmount;
        SegmentSpace = segmentSpace;
    }

    public void SpawnSegments<TBody, TTail>(IEntitySource source, Player player, Vector2 position, int damage, float knockBack)
        where TBody : BaseProjectile
        where TTail : BaseProjectile
    {
        PastPosition = position;

        if (BodyType != WormBodyType.Head)
        {
            throw new InvalidOperationException($"Only {WormBodyType.Head.ToString()} body type can spawn segments");
        }

        var parent = this;
        for (int i = 0; i < SegmentAmount; i++)
        {
            var module = SpawnSingle<TBody>(parent, source, player, position, damage, knockBack);
            parent = module;
        }

        SpawnSingle<TTail>(parent, source, player, position, damage, knockBack);
    }

    private WormModule SpawnSingle<T>(WormModule parent, IEntitySource source, Player player, Vector2 position, int damage, float knockBack)
        where T : BaseProjectile
    {
        var segment = Helper.SpawnProjectile<T>(source, player, position, Vector2.Zero, damage, knockBack);

        if (segment.Composite.TryGetModule(out WormModule? module))
        {
            module.Parent = parent;
            return module;
        }
        else
        {
            throw new InvalidOperationException($"{segment.Name} does not have worm module");
        }
    }

    public (Vector2, float) Follow(Vector2 position, float rotation)
    {
        PastPosition = position;
        PastRotation = rotation;

        if (BodyType == WormBodyType.Head)
        {
            return (Vector2.Zero, 0f);
        }

        return (Parent!.PastPosition - Vector2.UnitY.RotatedBy(Parent.PastRotation).RotatedBy(MathHelper.Pi) * SegmentSpace, Parent.PastRotation);
        // return (Parent!.PastPosition, Parent.PastRotation);
    }
}