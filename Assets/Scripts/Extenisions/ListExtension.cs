using System;
using System.Collections.Generic;

public static class ListExtension
{
    public static T Random<T>(this List<T> list)
    {
        var rand = new Random();
        var obj = list[rand.Next(0, list.Count)];
        return obj;
    }
}