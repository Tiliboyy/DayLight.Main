using System;

namespace DayLight.Dependency.Models.Helpers;

[Serializable]
public class BoolHelper
{
    public bool Bool { get; set; }
    public BoolHelper(bool state)
    {
        Bool = state;
    }
}
