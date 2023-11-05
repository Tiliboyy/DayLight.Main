using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace DayLight.Hints.Configs;

[Automatic]
public class HintsTranslation : Translations<HintsTranslation>
{
    public string Title { get; set; } = "<align=right> <size=45%><color=(COLOR)><b>👥 Zuschauer ((COUNT)):</b></color></size></align>";
    public string Names { get; set; } = "<align=right><size=45%><color=(COLOR)>(NAME)</color></size></align>";

    public string Kills { get; set; } = " Kills: (KILLS)";
        
    public string ProximityChatText { get; set; } = "<color=yellow> Proximity Chat</color>";

}