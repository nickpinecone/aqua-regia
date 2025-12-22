using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Fluent;

public class ProjectileSpawner<T>
    where T : ModProjectile
{
    private IEntitySource? _source;
    private Player? _player;

    private Vector2 _position = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;

    private int _damage = 0;
    private float _knockBack = 0f;

    public T Spawn()
    {
        ArgumentNullException.ThrowIfNull(_source);
        ArgumentNullException.ThrowIfNull(_player);

        var type = ModContent.ProjectileType<T>();
        var proj = Projectile.NewProjectileDirect(_source, _position, _velocity, type, _damage, _knockBack,
            _player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public ProjectileSpawner<T> Context(IEntitySource source, Player player)
    {
        _source = source;
        _player = player;
        return this;
    }

    public ProjectileSpawner<T> Position(Vector2 position)
    {
        _position = position;
        return this;
    }

    public ProjectileSpawner<T> Velocity(Vector2 velocity)
    {
        _velocity = velocity;
        return this;
    }

    public ProjectileSpawner<T> Damage(int damage, float knockBack)
    {
        _damage = damage;
        _knockBack = knockBack;
        return this;
    }
}