#region

using Neuron.Core.Meta;
using Syml;
using System;
using System.ComponentModel;
using IConfig = DayLight.Core.IConfig;

#endregion

namespace DayLight.Stat;

[Serializable]
[Automatic]
[DocumentSection("DayLight.Stats")]
public class Config : IConfig
{
    public ushort BroadcastTime { get; set; } = 10;
    [Description("Enables the Plugin")]
    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
