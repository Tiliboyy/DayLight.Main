using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace DayLight.Core;

[Automatic]
public class Translation : Translations<Translation>
{ 
    public string CurrencyName { get; set; }
}
