using AquaRegia.Library;
using AquaRegia.Library.Extended.Fluent;
using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Items;
using AquaRegia.Library.Extended.Sources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.WoodenTrident;

public class WoodenTrident : BaseItem
{
    public override string Texture => Assets.Sprites.Weapons.WoodenTrident.WoodenTridentItem;

    private PropertyModule Property { get; } = new();

    [RuntimeModule] private SpearModule Spear { get; } = new();

    public override void SetDefaults()
    {
        Property.Set(this)
            .Damage(25, 6.5f, DamageClass.Melee)
            .UseStyle(ItemUseStyleID.Shoot, 18, 12)
            .UseSound(SoundID.Item71)
            .Shoot<WoodenTridentProjectile>(AmmoID.None, 3.7f)
            .Hide(true)
            .Rarity(ItemRarityID.Pink)
            .Price(Item.sellPrice(silver: 10));
    }

    public override void AltUseAlways(Player player)
    {
        base.AltUseAlways(player);

        if (player.ownedProjectileCounts[Item.shoot] < 1 &&
            player.ownedProjectileCounts[ModContent.ProjectileType<WoodenTridentThrow>()] < 1)
        {
            new ProjectileSpawner<WoodenTridentThrow>()
                .Context(new WeaponWithAmmoSource(this), player)
                .Damage(Item.damage, Item.knockBack)
                .Position(player.Center)
                .Velocity((Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 12f)
                .Spawn();
        }
        else
        {
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.ModProjectile is WoodenTridentThrow tridentThrow)
                {
                    tridentThrow.IsRecalled = true;
                }
            }
        }
    }

    public override bool CanUseItem(Player player)
    {
        return base.CanUseItem(player) &&
               player.ownedProjectileCounts[ModContent.ProjectileType<WoodenTridentThrow>()] < 1;
    }
}