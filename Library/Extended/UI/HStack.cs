using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace AquaRegia.Library.Extended.UI;

public class HStack : UIElement
{
    public int Gap { get; set; }

    private readonly List<UIElement> _children = [];
    private float _maxHeight = 0f;

    public HStack(int gap)
    {
        Gap = gap;
    }

    public void AddElement(UIElement element)
    {
        _children.Add(element);
        Append(element);
    }

    public void RemoveElement(UIElement element)
    {
        if (!_children.Contains(element)) return;

        _children.Remove(element);
        RemoveChild(element);
    }

    private void AdjustHeight()
    {
        _maxHeight = _children.Max(ch => ch.Height.Pixels);
        Height = StyleDimension.FromPixels(_maxHeight + PaddingTop + PaddingBottom);
    }

    private void AdjustWidth()
    {
        Width.Pixels = PaddingLeft + PaddingRight;
        for (var i = 0; i < _children.Count; i++)
        {
            var child = _children[i];
            Width.Pixels += child.Width.Pixels;

            if (i > 0)
            {
                Width.Pixels += Gap;
            }
        }
    }

    private void PositionElements()
    {
        var offset = 0f;
        foreach (var child in _children)
        {
            child.Left.Pixels = offset;
            offset += child.Width.Pixels + Gap;
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        AdjustHeight();
        AdjustWidth();
        PositionElements();
    }
}