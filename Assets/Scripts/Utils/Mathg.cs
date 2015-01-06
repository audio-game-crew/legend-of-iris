using UnityEngine;
using System.Collections;

public static class Mathg {

    public const float SQRT2 = 1.4142135623730950488016887242097f;
    public const float SQRT2Inv = 0.70710678118654752440084436210485f;

    /// <summary>
    /// Maps a position in range [-1, 1] to a circle within [-1, 1]
    /// </summary>
    public static void Circular(ref float x, ref float y)
    {
        float _x = x;
        x = x * Mathf.Sqrt(1f - 0.5f * y * y);
        y = y * Mathf.Sqrt(1f - 0.5f * _x * _x);
    }
    /// <summary>
    /// Maps a position in range [-1, 1] to a circular direction with max length 1
    /// </summary>
    public static Vector3 CircularDirection(float x, float y)
    {
        Circular(ref x, ref y);
        return new Vector3(x, y);
    }

    public static void Swap(ref float v1, ref float v2)
    {
        float temp = v2;
        v2 = v1;
        v1 = temp;
    }

    public static float Normalize(this float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public static float Interpolate(this float value, float min, float max)
    {
        return value * (max - min) + min;
    }
}
