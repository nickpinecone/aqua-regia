using AquaRegia.UI;
using AquaRegia.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AquaRegia.Weapons.Sunflower;

public class SunflowerPlayer : ModPlayer
{
    public Sunflower Sunflower = null;
    public BloodVine BloodVine = null;

    private GaugeElement _gauge = null;
    private Timer _updateTimer = new Timer(20);

    public void SpawnSunflower(int damage, float knockback)
    {
        var proj =
            Projectile.NewProjectileDirect(Projectile.GetSource_None(), Main.LocalPlayer.Center, Vector2.Zero,
                                           ModContent.ProjectileType<Sunflower>(), damage, knockback, Main.LocalPlayer.whoAmI);
        Sunflower = proj.ModProjectile as Sunflower;

        _gauge = ModContent.GetInstance<InterfaceSystem>().CreateGauge("Sunflower Gauge", 60, 60);
        _gauge.ColorBorderFull = Color.White;
        if (Main.IsItDay())
        {
            _gauge.ColorA = Color.Gold;
            _gauge.ColorB = Color.Yellow;
        }
        else
        {
            _gauge.ColorA = Color.DarkRed;
            _gauge.ColorB = Color.Red;
        }
    }

    public override void PreUpdate()
    {
        base.PreUpdate();

        if (_gauge != null)
        {
            Sunflower.Projectile.timeLeft = 11;

            _updateTimer.Update();

            if (_updateTimer.Done)
            {
                _updateTimer.Restart();

                _gauge.Current -= 1;

                if (_gauge.Current <= 0)
                {
                    ModContent.GetInstance<InterfaceSystem>().RemoveGauge(_gauge);
                    _gauge = null;
                }
            }
        }
    }

    public override void ModifyDrawInfo(ref Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        base.ModifyDrawInfo(ref drawInfo);

        if (Sunflower != null)
        {
            var position = Helper.ToVector2I(drawInfo.Center - Sunflower.Offset);

            Sunflower.Projectile.Center = position;
        }
    }

    public override void UpdateEquips()
    {
        if (Sunflower != null && Main.IsItDay())
        {
            Main.LocalPlayer.statDefense += 8;
            Main.LocalPlayer.GetDamage(DamageClass.Ranged) += 0.2f;
        }
    }
}
