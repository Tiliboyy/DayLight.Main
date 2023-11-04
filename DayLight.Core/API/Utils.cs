using DayLight.Core.API.Features;
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
    public static string GetColorHexCode(string color) => !Enum.TryParse(color, true, out Misc.PlayerInfoColorTypes colorEnum)
        ? Colors[Misc.PlayerInfoColorTypes.White]
        : Colors[colorEnum];
    public static Dictionary<Misc.PlayerInfoColorTypes, string> Colors { get; } = Misc.AllowedColors;
}
