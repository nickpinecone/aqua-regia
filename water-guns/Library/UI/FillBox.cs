using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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

    private int _current;

    public int Current
    {
        get => _current;
        set => _current = Math.Clamp(value, 0, Max);
    }

    public int Max { get; set; }
    public int BorderWidth { get; set; }

    public string Tooltip { get; set; }

    public Color ColorA { get; set; }
    public Color ColorB { get; set; }
    public Color ColorBorder { get; set; }

    public FillBox(StyleDimension width, StyleDimension height, int max, int borderWidth, string tooltip,
        Color? colorA = null, Color? colorB = null, Color? colorBorder = null)
    {
        Width = width;
        Height = height;
        Max = Math.Max(0, max);
        BorderWidth = borderWidth;
        Tooltip = tooltip;

        ColorA = colorA ?? Color.Blue;
        ColorB = colorB ?? Color.Cyan;
        ColorBorder = colorBorder ?? Color.White;
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

    private void DisplayTooltip()
    {
        if (IsMouseHovering)
        {
            Main.hoverItemName = Tooltip;
        }
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

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        DisplayTooltip();
        DrawBorder(spriteBatch);
        DrawFill(spriteBatch);
    }
}