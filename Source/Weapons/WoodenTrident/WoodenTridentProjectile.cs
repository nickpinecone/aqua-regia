using System.Numerics;
using AquaRegia.Library;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Projectiles;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTridentProjectile : BaseProjectile
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.WoodenTridentProjectile;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private SpearModule Spear { get; } = new();

    public override void SetDefaults()
    {
        Spear.SetDefaults();

        Property.Set(this)
            .Size(18, 18, 1.2f)
            .Friendly(true, false)
            .Damage(DamageClass.Melee, -1)
            .TileCollide(false, true)
            .TimeLeft(int.MaxValue)
            .DrawOffset(0, drawOriginOffset: new Vector2(-26, 0))
            .Hide(true);
    }
}