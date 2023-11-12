using System;

namespace DayLight.Dependencys.Communication;

[Serializable]
public struct LinkMessage
{
    public ulong UserId { get; set; }
    public int Code { get; set; }
    public bool Linked { get; set; }
}
