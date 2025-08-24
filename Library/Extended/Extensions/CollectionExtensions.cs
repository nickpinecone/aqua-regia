using System.Collections.Generic;
using Terraria;

namespace AquaRegia.Library.Extensions;

public static class CollectionExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = Main.rand.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}