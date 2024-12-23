using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AquaRegia.Modules.Weapons;
using static AquaRegia.AquaRegia;

namespace AquaRegia.Modules;

public abstract class BaseGun : ModItem, IComposite<BaseGunModule>
{
    protected Dictionary<Type, BaseGunModule> _modules = new();
    Dictionary<Type, BaseGunModule> IComposite<BaseGunModule>.Modules => _modules;
    List<BaseGunModule> IComposite<BaseGunModule>.RuntimeModules => throw new NotImplementedException();

    public SpriteModule Sprite { get; private set; }

    protected BaseGun()
    {
        Sprite = new SpriteModule(this);
    }

    public T SpawnProjectile<T>(Player player, Vector2 position, Vector2 velocity, int damage, float knockback,
                                ProjectileSource customSource = null)
        where T : BaseProjectile
    {
        var projSource = new AquaRegia.ProjectileSource(Projectile.GetSource_NaturalSpawn())
        {
            Weapon = this,
            Ammo = null,
        };

        if (customSource != null)
        {
            customSource.Inherit(projSource);
        }

        var type = ModContent.ProjectileType<T>();

        var proj = Projectile.NewProjectileDirect(customSource ?? projSource, position, velocity, type, damage,
                                                  knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public T ShootProjectile<T>(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
                                int damage, float knockback, ProjectileSource customSource = null)
        where T : BaseProjectile
    {
        var ammo = (BaseAmmo)ModContent.GetModItem(source.AmmoItemIdUsed);
        var projSource = new AquaRegia.ProjectileSource(source)
        {
            Weapon = this,
            Ammo = ammo,
        };

        if (customSource != null)
        {
            customSource.Inherit(projSource);
        }

        var type = ModContent.ProjectileType<T>();

        var proj = Projectile.NewProjectileDirect(customSource ?? projSource, position, velocity, type, damage,
                                                  knockback, player.whoAmI);
        return (T)proj.ModProjectile;
    }

    public override bool AltFunctionUse(Player player)
    {
        AltUseAlways(player);

        return base.AltFunctionUse(player);
    }

    public void DoAltUse(Player player)
    {
        if (Main.mouseLeft && Main.mouseRight)
        {
            AltUseAlways(player);
        }
    }

    public virtual void AltUseAlways(Player player)
    {
    }

    public override Vector2? HoldoutOffset()
    {
        return Sprite.HoldoutOffset;
    }
}
