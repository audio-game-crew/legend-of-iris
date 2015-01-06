
using UnityEngine;
public static class Randomg
{
    public static bool Boolean()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }
    public static float Range01()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }
    public static float Range(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    public static int Range(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    public static int Range(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }
    public static float Range(float max)
    {
        return UnityEngine.Random.Range(0, max);
    }
    public static int Symmetrical(int range)
    {
        return UnityEngine.Random.Range(-range, range + 1);
    }
    public static float Symmetrical(float range)
    {
        return UnityEngine.Random.Range(-range, range);
    }
    public static int Symmetrical(int around, int range)
    {
        return UnityEngine.Random.Range(around - range, around + range + 1);
    }
    public static float Symmetrical(float around, float range)
    {
        return UnityEngine.Random.Range(around - range, around + range);
    }
    public static bool Picker(float border, float max)
    {
        return Range(max) < border;
    }
    public static bool Picker(int border, int max)
    {
        return Range(max) < border;
    }
    public static bool Picker(float border)
    {
        return Range01() < border;
    }
    public static int Buckets(int max, params int[] values)
    {
        int r = Range(max);
        int current = 0;
        foreach (int f in values)
        {
            if (r < f)
            {
                return current;
            }
            current++;
        }
        return current;
    }
    public static int Buckets(float max, params float[] values)
    {
        float r = Range(max);
        int current = 0;
        foreach (float f in values)
        {
            if (r < f)
            {
                return current;
            }
            current++;
        }
        return current;
    }

    public static float Gaussian()
    {
        return Gaussian(0f, 0f);
    }

    public static float Gaussian(float standardDeviation)
    {
        return Gaussian(0f, standardDeviation);
    }

    public static float Gaussian(float mean, float standardDeviation)
    {
        float u1 = Range01(); //these are uniform(0,1) random doubles
        float u2 = Range01();
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        return mean + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)
    }
}