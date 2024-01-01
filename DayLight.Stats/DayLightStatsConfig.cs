#region

using Neuron.Core.Meta;
using Syml;
using System;
using System.ComponentModel;
using IConfig = DayLight.Core.IConfig;

#endregion

namespace DayLight.Stats;

[Serializable]
[Automatic]
[DocumentSection("DayLight.Stats")]
public class DayLightStatsConfig : IConfig
{
    public ushort BroadcastTime { get; set; } = 10;
    [Description("Enables the Plugin")]
    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
