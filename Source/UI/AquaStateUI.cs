using AquaRegia.Library.Extended.Modules;
using AquaRegia.Library.Extended.Modules.Shared;
using AquaRegia.Library.Extended.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace AquaRegia.UI;

public class AquaStateUI : UIState
{
    public HStack BoxContainer { get; set; } = null!;
    public FillBox PrimaryBox { get; set; } = null!;

    public override void OnInitialize()
    {
        base.OnInitialize();

        PrimaryBox = new FillBox(StyleDimension.FromPixels(18), StyleDimension.FromPixels(90), 0, 2, "Primary")
        {
            HAlign = 0.99f,
            VAlign = 0.98f
        };

        BoxContainer = new HStack(8)
        {
            HAlign = 0.97f,
            VAlign = 0.98f,
        };

        Append(PrimaryBox);
        Append(BoxContainer);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (Main.LocalPlayer.HeldItem.ModItem is BaseItem weapon &&
            weapon.Composite.TryGetModule<ProgressModule>(out var progressModule))
        {
            PrimaryBox.Hidden = false;

            PrimaryBox.Max = progressModule.Timer.Duration;
            PrimaryBox.ColorBorder = Color.White;
            PrimaryBox.Current = progressModule.Timer.Current;
            PrimaryBox.ColorBorder = progressModule.Done ? Color.Gold : Color.White;
        }
        else
        {
            PrimaryBox.Hidden = true;
        }
    }
}