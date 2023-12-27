using Neuron.Core.Meta;
using PlayerRoles;
using Syml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using IConfig = DayLight.Core.IConfig;

namespace DayLight.Hints.Configs;

[Serializable]
[Automatic]
[DocumentSection("DayLight.Hints")]
public class HintsConfig : IConfig
{

    [Description("Enables the Plugin")]
    public bool Enabled { get; set; } = true;

    public bool Debug { get; set; } = false;
    
    public int SpectatorLimit { get; set; } = 15;
    public List<RoleTypeId> KillCounterRoles { get; set; } = new()
    {
        RoleTypeId.Scp049,
        RoleTypeId.Scp079,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp173,
        RoleTypeId.Scp0492,
        RoleTypeId.Scp939,
    };
}