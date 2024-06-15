using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WaterGuns.Ammo;
using WaterGuns.Projectiles;
using WaterGuns.Utils;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.Weapons;

public abstract class BaseGun : ModItem
{
    private List<Timer> _timers = new();

    protected Dictionary<Type, BaseGunModule> _modules = new();
    protected SpriteModule _sprite;

    protected BaseGun()
    {
        _sprite = new SpriteModule(this);
    }

    public bool HasModule<T>()
    {
        return _modules.ContainsKey(typeof(T));
    }

    public void AddModule(BaseGunModule module)
    {
        _modules[module.GetType()] = module;
    }

    public T GetModule<T>()
        where T : BaseGunModule
    {
        return (T)_modules[typeof(T)];
    }

    public T ShootProjectile<T>(
        Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int damage, int knockback
    ) where T : BaseProjectile
    {
        var ammo = (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed);
        var projSource = new WaterGuns.ProjectileSource(source)
        {
            Weapon = this,
            Ammo = ammo,
        };

        var type = ModContent.ProjectileType<T>();

        var proj = Projectile.NewProjectileDirect(projSource, position, velocity, type, damage, knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public override void HoldItem(Player player)
    {
        base.HoldItem(player);

        foreach (var timer in _timers)
        {
            timer.Update();
        }
    }

    public override Vector2? HoldoutOffset()
    {
        return _sprite.HoldoutOffset;
    }
}