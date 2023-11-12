using System;
using System.Collections.Generic;

namespace DayLight.Dependencys.Utils;

public class TimeSpanUtils
{
    public static string FormatTimeSpan(TimeSpan ts, bool useSeconds = false)
    {
        if (ts == TimeSpan.Zero)
            return "N/A";
        var parts = new List<string>();

        if (ts.Days > 0)
            parts.Add($"{ts.Days} {(ts.Days == 1 ? "Tag" : "Tage")}");

        if (ts.Hours > 0)
            parts.Add($"{ts.Hours} {(ts.Hours == 1 ? "Stunde" : "Stunden")}");

        if (ts.Minutes > 0)
            parts.Add($"{ts.Minutes} {(ts.Minutes == 1 ? "Minute" : "Minuten")}");

        if (useSeconds && ts.Seconds > 0)
            parts.Add($"{ts.Seconds} {(ts.Seconds == 1 ? "Sekunde" : "Sekunden")}");

        return string.Join(", ", parts);
    }
}
