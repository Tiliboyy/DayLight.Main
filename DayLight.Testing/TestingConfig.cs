using Neuron.Core.Meta;
using Syml;

namespace DayLight.Test;

[Automatic]
[DocumentSection("DayLight.Testing")]
public class TestConfig : Core.IConfig
{

    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public bool FreeRoles { get; set; } = false;
}

