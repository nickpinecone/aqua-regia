using Microsoft.Xna.Framework;
using WaterGuns.Projectiles;
using WaterGuns.Weapons.Modules;

namespace WaterGuns.Weapons;

public class WoodenGun : BaseGun
{
    public override string Texture => "WaterGuns/Assets/Textures/Weapons/WoodenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }

    public WoodenGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetDefaults();
        Property.SetDefaults();
        Property.SetProjectile<WoodenProjectile>();

        Sprite.HoldoutOffset = new Vector2(0, 5);
        Sprite.Offset = new Vector2(24f, 24f);
        Pump.MaxPumpLevel = 8;
        Property.Inaccuracy = 3.5f;

        Item.width = 38;
        Item.height = 22;
        Item.damage = 5;
        Item.knockBack = 0.8f;
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        Pump.DefaultUpdate();
    }

    public override bool AltFunctionUse(Terraria.Player player)
    {
        base.AltFunctionUse(player);

        if(Pump.Pumped)
        {
            Pump.Reset();
        }

        return true;
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
    {
        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);

        var projectile = ShootProjectile<WoodenProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }
}
