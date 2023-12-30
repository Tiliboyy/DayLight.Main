using System;

namespace DayLight.Dependencys.Models.Helpers;

[Serializable]
public class BoolHelper
{
    public bool Bool { get; set; }
    public BoolHelper(bool state)
    {
        Bool = state;
    }
}
