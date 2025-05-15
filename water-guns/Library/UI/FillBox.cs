using System.Security.Cryptography.Pkcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace WaterGuns.Library.UI;

public class FillBox : UIElement
{
    private enum BorderSides
    {
        Top,
        Bottom,
        Left,
        Right,
        Count
    }

    public int Current { get; set; }
    public int Max { get; set; }
    public int BorderWidth { get; set; }
    
    public Color ColorA { get; set; } = Color.Blue;
    public Color ColorB { get; set; } = Color.Cyan;
    public Color ColorBorder { get; set; } = Color.White;

    public FillBox(StyleDimension width, StyleDimension height, int max, int borderWidth)
    {
        Width = width;
        Height = height;
        Max = max;
        BorderWidth = borderWidth;
    }

    private new Rectangle GetInnerDimensions()
    {
        var inner = base.GetInnerDimensions();

        return new Rectangle(
            (int)(inner.X + BorderWidth),
            (int)(inner.Y + BorderWidth),
            (int)(inner.Width - BorderWidth * 2),
            (int)(inner.Height - BorderWidth * 2)
        );
    }

    private void DrawBorder(SpriteBatch spriteBatch)
    {
        var rect = GetDimensions();
        var borders = new Rectangle[(int)BorderSides.Count];

        borders[(int)BorderSides.Top] = new Rectangle(
            (int)rect.X, (int)rect.Y,
            (int)rect.Width, BorderWidth
        );

        borders[(int)BorderSides.Bottom] = new Rectangle(
            (int)rect.X, (int)(rect.Y + rect.Height - BorderWidth),
            (int)rect.Width, BorderWidth
        );

        borders[(int)BorderSides.Left] = new Rectangle(
            (int)rect.X, (int)rect.Y,
            BorderWidth, (int)rect.Height
        );

        borders[(int)BorderSides.Right] = new Rectangle(
            (int)(rect.X + rect.Width - BorderWidth),
            (int)rect.Y, BorderWidth, (int)rect.Height
        );

        foreach (var border in borders)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, border, ColorBorder);
        }
    }

    private void DrawFill(SpriteBatch spriteBatch)
    {
        var percent = Current / (float)Max;
        var rect = GetInnerDimensions();
        var steps = (int)((rect.Bottom - rect.Top) * percent);

        for (var i = 0; i < steps; i += 1)
        {
            var gradient = (float)i / (rect.Bottom - rect.Top);
            
            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                new Rectangle(rect.X, rect.Y + rect.Height - i, rect.Width, 1),
                Color.Lerp(ColorA, ColorB, gradient)
            );
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        DrawBorder(spriteBatch);
        DrawFill(spriteBatch);
    }
}