using System;

namespace DayLight.DiscordSync.Dependencys.Communication;

[Serializable]
public struct LinkMessage
{
    public ulong UserId { get; set; }
    public int Code { get; set; }
    public bool Linked { get; set; }
}
