using System;

namespace AquaRegia.Library.Tween.Ease;

public static class Ease
{
    public static Func<float, float> Linear => (x) => x;

    // Ease In Quad
    public static Func<float, float> In => (x) => x * x;

    // Ease Out Quad
    public static Func<float, float> Out => (x) => 1f - (1f - x) * (1f - x);

    // Ease In And Out Quad
    public static Func<float, float> InOut => (x) => x < 0.5f ? 2f * x * x : 1f - MathF.Pow(-2f * x + 2f, 2f) / 2f;
}