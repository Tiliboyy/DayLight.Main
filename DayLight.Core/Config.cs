using Neuron.Core.Meta;
using Syml;

namespace DayLight.Core;

[Automatic]
[DocumentSection("DayLight.Core")]
public class Config : IConfig
{

    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
