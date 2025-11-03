namespace AquaRegia.Library.Extended.Modules.Items;

public class PropertyModule : IModule
{
    private BaseItem _item;

    public PropertyModule(BaseItem item)
    {
        _item = item;
    }

    public PropertyModule Size(int width, int height)
    {
        _item.Item.width = width;
        _item.Item.height = height;
        return this;
    }

    public PropertyModule Rarity(int rare)
    {
        _item.Item.rare = rare;
        return this;
    }
}