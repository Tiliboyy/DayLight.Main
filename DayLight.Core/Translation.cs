using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;
using Syml;

namespace DayLight.Core;

[Automatic]
public class Translation : Translations<Translation>
{

    public string CurrencyName { get; set; }
}
