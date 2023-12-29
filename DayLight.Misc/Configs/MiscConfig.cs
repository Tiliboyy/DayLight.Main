using Neuron.Core.Meta;
using Syml;
using System.ComponentModel;

namespace DayLight.Misc.Configs;

[Automatic]
[DocumentSection("DayLight.Testing")]
public class MiscConfig : Core.IConfig
{

    public bool Enabled { get; set; } = true;
    
    public bool Debug { get; set; } = false;
    
    [Description("Adrenalin Speedboost")]
    public bool AdrenalinSpeedboost { get; set; } = true;
    public float SpeedDuration { get; set; } = 5;
    public byte SpeedIntensity { get; set; } = 50;
    
    public bool StackableSpeed { get; set; } = true;
    public byte MaxStackedSpeed { get; set; } = 254;



    [Description("Cola goes Boom")]
    

    public int Scp207ExplosionAmount { get; set; } = 4;
    public int Scp207ExplosionGrenadeAmount { get; set; } = 4;
    public float FuseTime { get; set; } = 0.75f;
 
    [Description("Auto FriendlyFire Toggle")]
    public bool AutoFriendlyFireToggle { get; set; } = true;
}

