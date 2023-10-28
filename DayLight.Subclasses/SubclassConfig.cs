using DayLight.Subclasses.Subclasses;
using Exiled.API.Features;
using Exiled.Loader;
using Neuron.Core.Meta;
using Syml;
using System.ComponentModel;
using System.IO;
using IConfig = DayLight.Core.IConfig;

namespace DayLight.Subclasses;

[Automatic]
[DocumentSection("DayLight.Subclasses")]
public class SubclassConfig : IConfig
{
    
    
    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;

}