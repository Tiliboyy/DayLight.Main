using Neuron.Core.Meta;
using Syml;

namespace DayLight.Core;

[Automatic]
[DocumentSection("DayLight.Core")]
public class Config : IConfig
{

    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public string BoughtItemHint { get; set; }
    public int MoneyMuliplier { get; set; } = 1;
    public bool EnableLimit { get; set; } = false;
    public float MoneyLimit { get; set; } = 500000000;
}
