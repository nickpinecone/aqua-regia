using AquaRegia.Ammo;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Items;

public class PropertyModule : IModule
{
    public PropertyDefaults Defaults { get; private set; } = null!;
    
    private BaseItem _base = null!;

    public PropertyModule Set(BaseItem baseItem)
    {
        _base = baseItem;
        Defaults = new PropertyDefaults(this);
        return this;
    }

    public PropertyModule Width(int width)
    {
        _base.Item.width = width;
        return this;
    }

    public PropertyModule Height(int height)
    {
        _base.Item.height = height;
        return this;
    }

    public PropertyModule Size(int width, int height)
    {
        _base.Item.width = width;
        _base.Item.height = height;
        return this;
    }

    public PropertyModule KnockBack(float knockBack)
    {
        _base.Item.knockBack = knockBack;
        return this;
    }

    public PropertyModule DamageType(DamageClass damageType)
    {
        _base.Item.DamageType = damageType;
        return this;
    }

    public PropertyModule Damage(int damage)
    {
        _base.Item.damage = damage;
        return this;
    }

    public PropertyModule Damage(int damage, float knockBack)
    {
        _base.Item.damage = damage;
        _base.Item.knockBack = knockBack;
        return this;
    }

    public PropertyModule Damage(DamageClass damageType, int damage, float knockBack)
    {
        _base.Item.DamageType = damageType;
        _base.Item.damage = damage;
        _base.Item.knockBack = knockBack;
        return this;
    }

    public PropertyModule Ammo(int ammo)
    {
        _base.Item.ammo = ammo;
        return this;
    }

    public PropertyModule UseAmmo(int ammo)
    {
        _base.Item.useAmmo = ammo;
        return this;
    }

    public PropertyModule ShootSpeed(float shootSpeed)
    {
        _base.Item.shootSpeed = shootSpeed;
        return this;
    }

    public PropertyModule Melee(bool melee)
    {
        _base.Item.noMelee = !melee;
        return this;
    }

    public PropertyModule Shoot<T>()
        where T : BaseProjectile
    {
        _base.Item.noMelee = true;
        _base.Item.shoot = ModContent.ProjectileType<T>();
        return this;
    }

    public PropertyModule Shoot<T>(float shootSpeed)
        where T : BaseProjectile
    {
        _base.Item.noMelee = true;
        _base.Item.shootSpeed = shootSpeed;
        _base.Item.shoot = ModContent.ProjectileType<T>();
        return this;
    }

    public PropertyModule Shoot<T>(int ammo, float shootSpeed)
        where T : BaseProjectile
    {
        _base.Item.noMelee = true;
        _base.Item.useAmmo = ammo;
        _base.Item.shootSpeed = shootSpeed;
        _base.Item.shoot = ModContent.ProjectileType<T>();
        return this;
    }

    public PropertyModule UseStyle(int useStyle)
    {
        _base.Item.useStyle = useStyle;
        return this;
    }

    public PropertyModule UseAnimation(int useAnimation)
    {
        _base.Item.useAnimation = useAnimation;
        return this;
    }

    public PropertyModule AutoReuse(bool autoReuse = true)
    {
        _base.Item.autoReuse = autoReuse;
        return this;
    }

    public PropertyModule UseTime(int useTime)
    {
        _base.Item.useTime = useTime;
        return this;
    }

    public PropertyModule UseTime(int useTime, int useAnimation)
    {
        _base.Item.useTime = useTime;
        _base.Item.useAnimation = useAnimation;
        return this;
    }

    public PropertyModule UseStyle(int useStyle, int useTime, int useAnimation, bool autoReuse = true)
    {
        _base.Item.useStyle = useStyle;
        _base.Item.useTime = useTime;
        _base.Item.useAnimation = useAnimation;
        _base.Item.autoReuse = autoReuse;
        return this;
    }

    public PropertyModule UseSound(SoundStyle useSound)
    {
        _base.Item.UseSound = useSound;
        return this;
    }

    public PropertyModule Rarity(int rare)
    {
        _base.Item.rare = rare;
        return this;
    }

    public PropertyModule Price(int value)
    {
        _base.Item.value = value;
        return this;
    }

    public PropertyModule Consumable(bool consumable)
    {
        _base.Item.consumable = consumable;
        return this;
    }

    public PropertyModule MaxStack(int maxStack, bool consumable = false)
    {
        _base.Item.maxStack = maxStack;
        _base.Item.consumable = consumable;
        return this;
    }

    public PropertyModule Hide(bool noUseGraphic)
    {
        _base.Item.noUseGraphic = noUseGraphic;
        return this;
    }

    public class PropertyDefaults(PropertyModule propertyModule)
    {
        private static readonly SoundStyle WaterShootSound = new(Assets.Audio.Use.water_shoot)
        {
            Pitch = -0.1f,
            PitchVariance = 0.1f,
        };

        public PropertyModule BottledWater() => propertyModule
            .DamageType(DamageClass.Ranged)
            .Ammo(ModContent.ItemType<BottledWater>())
            .MaxStack(Item.CommonMaxStack, true);

        public PropertyModule WaterGun() => propertyModule
            .DamageType(DamageClass.Ranged)
            .UseAmmo(ModContent.ItemType<BottledWater>())
            .UseSound(WaterShootSound)
            .UseStyle(ItemUseStyleID.Shoot);

        public PropertyModule Spear() => propertyModule
            .DamageType(DamageClass.Melee)
            .Melee(false)
            .ShootSpeed(1f)
            .UseStyle(ItemUseStyleID.Shoot)
            .UseSound(SoundID.Item1)
            .Hide(true);
    }
}