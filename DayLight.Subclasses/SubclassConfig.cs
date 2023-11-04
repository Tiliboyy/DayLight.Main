using Neuron.Core.Meta;
using Syml;
using IConfig = DayLight.Core.IConfig;

namespace DayLight.Subclasses;

[Automatic]
[DocumentSection("DayLight.Subclasses")]
public class SubclassConfig : IConfig
{
    
    
    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;

}