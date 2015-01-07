using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ListExtension
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[list.GetRandomIndex<T>()];
    }
    public static int GetRandomIndex<T>(this List<T> list)
    {
        return Randomg.Range(0, list.Count);
    }
    public static T Shift<T>(this List<T> list)
    {

        T removable = default(T);
        if (list.Count > 0)
        {
            removable = list[0];
            list.RemoveAt(0);
        }
        return removable;
    }
    public static T Pop<T>(this List<T> list)
    {

        T removable = default(T);
        if (list.Count > 0)
        {
            removable = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
        return removable;
    }
    public static List<int> Fill(this List<int> list, int to)
    {
        for (int q = 0; q < to; q++)
        {
            list.Add(q);
        }
        return list;
    }
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int r = Randomg.Range(0, i);
            T temp = list[r];
            list[r] = list[i];
            list[i] = temp;
        }
    }
}
