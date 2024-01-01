using System;

namespace DayLight.Dependency.Models.Helpers;

[Serializable]
public struct LinkHelper
{
    public ulong UserId { get; set; }
    public int Code { get; set; }
    public bool Linked { get; set; }
}
