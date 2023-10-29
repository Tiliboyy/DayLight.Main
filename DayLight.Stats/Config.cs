#region

using Exiled.API.Interfaces;
using System;
using System.ComponentModel;

#endregion

namespace DayLight.Stat;

[Serializable]
public class Config : IConfig
{
    public ushort BroadcastTime { get; set; } = 10;
    [Description("Enables the Plugin")]
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
