using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API;

public static class Utils
{
    public static IEnumerable<T> GetValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();
}
