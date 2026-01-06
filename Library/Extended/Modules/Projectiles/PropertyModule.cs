using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AquaRegia.Library.Extended.Modules.Projectiles;

public class PropertyModule : IModule
{
    public PropertyDefaults Defaults { get; private set; } = null!;

    private BaseProjectile _base = null!;

    public PropertyModule Set(BaseProjectile baseProjectile)
    {
        _base = baseProjectile;
        Defaults = new PropertyDefaults(this);
        return this;
    }

    public PropertyModule Height(int height)
    {
        _base.Projectile.height = height;
        return this;
    }

    public PropertyModule Width(int width)
    {
        _base.Projectile.width = width;
        return this;
    }

    public PropertyModule Scale(float scale)
    {
        _base.Projectile.scale = scale;
        return this;
    }

    public PropertyModule Size(int width, int height, float scale = 1f)
    {
        _base.Projectile.width = width;
        _base.Projectile.height = height;
        _base.Projectile.scale = scale;
        return this;
    }

    public PropertyModule DamageType(DamageClass damageType)
    {
        _base.Projectile.DamageType = damageType;
        return this;
    }

    public PropertyModule Penetrate(int penetrate)
    {
        _base.Projectile.penetrate = penetrate;
        return this;
    }

    public PropertyModule CritChance(int critChance)
    {
        _base.Projectile.CritChance = critChance;
        return this;
    }

    public PropertyModule Damage(DamageClass damageType, int penetrate, int critChance = 0)
    {
        _base.Projectile.DamageType = damageType;
        _base.Projectile.penetrate = penetrate;
        _base.Projectile.CritChance = critChance;
        return this;
    }

    public PropertyModule TimeLeft(int timeLeft)
    {
        _base.Projectile.timeLeft = timeLeft;
        return this;
    }

    public PropertyModule Friendly(bool friendly, bool hostile)
    {
        _base.Projectile.friendly = friendly;
        _base.Projectile.hostile = hostile;
        return this;
    }

    public PropertyModule Alpha(int alpha)
    {
        _base.Projectile.alpha = alpha;
        return this;
    }

    public PropertyModule OwnerHitCheck(bool ownerHitCheck)
    {
        _base.Projectile.ownerHitCheck = ownerHitCheck;
        return this;
    }

    public PropertyModule TileCollide(bool tileCollide)
    {
        _base.Projectile.tileCollide = tileCollide;
        return this;
    }

    public PropertyModule GfxOffY(float gfxOffY)
    {
        _base.Projectile.gfxOffY = gfxOffY;
        return this;
    }

    public PropertyModule DrawOffsetX(int drawOffsetX)
    {
        _base.DrawOffsetX = drawOffsetX;
        return this;
    }

    public PropertyModule DrawOriginOffset(Vector2 drawOriginOffset)
    {
        _base.DrawOriginOffsetX = drawOriginOffset.X;
        _base.DrawOriginOffsetY = (int)drawOriginOffset.Y;
        return this;
    }

    public PropertyModule DrawOffset(int drawOffsetX = 0, Vector2? drawOriginOffset = null)
    {
        drawOriginOffset ??= Vector2.Zero;

        _base.DrawOffsetX = drawOffsetX;
        _base.DrawOriginOffsetX = drawOriginOffset.Value.X;
        _base.DrawOriginOffsetY = (int)drawOriginOffset.Value.Y;
        return this;
    }

    public PropertyModule DrawOffset(float gfxOffY, int drawOffsetX = 0, Vector2? drawOriginOffset = null)
    {
        drawOriginOffset ??= Vector2.Zero;

        _base.Projectile.gfxOffY = gfxOffY;
        _base.DrawOffsetX = drawOffsetX;
        _base.DrawOriginOffsetX = drawOriginOffset.Value.X;
        _base.DrawOriginOffsetY = (int)drawOriginOffset.Value.Y;
        return this;
    }

    public PropertyModule Hide(bool hide)
    {
        _base.Projectile.hide = hide;
        return this;
    }

    public class PropertyDefaults(PropertyModule propertyModule)
    {
        public PropertyModule Ranged() => propertyModule
            .Friendly(true, false)
            .DamageType(DamageClass.Ranged);

        public PropertyModule Melee() => propertyModule
            .Friendly(true, false)
            .DamageType(DamageClass.Melee);

        public PropertyModule Spear() => propertyModule
            .Friendly(true, false)
            .Damage(DamageClass.Melee, -1)
            .TileCollide(false)
            .OwnerHitCheck(true)
            .TimeLeft(int.MaxValue)
            .DrawOriginOffset(new Vector2(-26, 0))
            .Hide(true);

        public PropertyModule TridentThrow() => propertyModule
            .Friendly(true, false)
            .Damage(DamageClass.Melee, -1)
            .DrawOriginOffset(new Vector2(-26, 0))
            .TimeLeft(10);
    }
}