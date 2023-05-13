using System;

namespace MarvelTerrariaUniverse.Utilities.Extensions;
public static class MathHelperExtensions
{
    public static float Lerp(float value, float targetValue, float amount)
    {
        float result = value + (targetValue - value) * amount;

        return Math.Abs(targetValue - result) < amount ? targetValue : result;
    }

    public static int Step(int value, int targetValue, int step)
    {
        return value + Math.Sign(targetValue - value) * step;
    }
}
