using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API;

public static class Utils
{
    public static List<T> GetEnumValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}
