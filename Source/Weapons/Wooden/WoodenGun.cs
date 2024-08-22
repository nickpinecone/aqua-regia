using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AquaRegia.Utils;
using AquaRegia.Modules.Weapons;
using AquaRegia.Modules;
using AquaRegia.UI;
using System;

namespace AquaRegia.Weapons.Wooden;

public class WoodenGun : BaseGun
{
    public override string Texture => TexturesPath.Weapons + "Wooden/WoodenGun";

    public SoundModule Sound { get; private set; }
    public PropertyModule Property { get; private set; }
    public PumpModule Pump { get; private set; }
    public TreeBoostModule TreeBoost { get; private set; }

    private GaugeElement _gauge;
    private Timer _timer;

    public WoodenGun() : base()
    {
        Sound = new SoundModule(this);
        Property = new PropertyModule(this);
        Pump = new PumpModule(this);
        TreeBoost = new TreeBoostModule(this);

        _timer = new Timer(10);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Sound.SetWater(this);
        Property.SetDefaults(this);
        Property.SetProjectile<WoodenProjectile>(this);

        Sprite.HoldoutOffset = new Vector2(0, 6);
        Sprite.Offset = new Vector2(26f, 26f);
        Pump.MaxPumpLevel = 8;
        Property.Inaccuracy = 3.5f;

        Item.width = 38;
        Item.height = 22;
        Item.damage = 4;
        Item.knockBack = 0.8f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shootSpeed = 22f;

        Item.rare = ItemRarityID.White;
        Item.value = Item.sellPrice(0, 0, 0, 20);

        TreeBoost.Initialize(Item.damage, 2);
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        base.OnSpawn(source);
    }

    public override void OnCreated(Terraria.DataStructures.ItemCreationContext context)
    {
        base.OnCreated(context);

        ChatLog.Message("I was called");
    }

    public override void HoldItem(Terraria.Player player)
    {
        base.HoldItem(player);

        if (_gauge != null)
        {
            _timer.Update();

            if (_timer.Done)
            {
                _gauge.Current = Math.Min(_gauge.Current + 1, 100);
                _timer.Restart();
            }
        }

        Pump.DefaultUpdate();
        Item.damage = TreeBoost.Apply(player);

        DoAltUse(player);
    }

    public override void AltUseAlways(Player player)
    {
        if (Pump.Pumped)
        {
            var state = ModContent.GetInstance<InterfaceSystem>();
            _gauge = new GaugeElement(state._gaugeState.GetTexture());
            state._gaugeState.AddGauge(_gauge);
            _gauge.Max = 100;
            _gauge.Current = 0;

            SpawnProjectile<TreeProjectile>(player, Main.MouseWorld, Vector2.Zero, Item.damage * 2, Item.knockBack * 2);

            Pump.Reset();
        }
    }

    public override bool Shoot(Terraria.Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                               Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity,
                               int type, int damage, float knockback)
    {
        base.Shoot(player, source, position, velocity, type, damage, knockback);

        position = Sprite.ApplyOffset(position, velocity);
        velocity = Property.ApplyInaccuracy(velocity);

        ShootProjectile<WoodenProjectile>(player, source, position, velocity, damage, knockback);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Wood, 20);
        recipe.AddIngredient(ItemID.Acorn, 5);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltip)
    {
        base.ModifyTooltips(tooltip);

        Sprite.AddAmmoTooltip(tooltip, Mod);
    }
}
