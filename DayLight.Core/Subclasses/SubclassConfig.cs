using Neuron.Core.Meta;
using Syml;
using System.ComponentModel;

namespace DayLight.Core.Subclasses;

[Automatic]
[DocumentSection("DayLight.Core")]
public class SubclassConfig : DocumentSection
{
    public string CooldownText { get; set; } = "Deine Abiltiy ist noch für {Sekunden} Sekunden auf Cooldown";
    
    [Description("Used for Debugging")] 
    public bool AlwaysAllowSpawns { get; set; } = false;
    

}