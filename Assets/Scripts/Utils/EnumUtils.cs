using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumUtils
{
    public static List<T> GetValuesList<T>()
    {
        List<T> values = new List<T>();
        System.Array arr = Enum.GetValues(typeof(T));
        foreach (T val in arr)
        {
            values.Add(val);
        }
        return values;
    }
    public static Array GetValues<T>()
    {
        return Enum.GetValues(typeof(T));
    }
}
