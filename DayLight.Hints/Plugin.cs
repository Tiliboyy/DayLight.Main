using DayLight.Core.API;
using DayLight.Hints.Configs;
using Neuron.Core.Plugins;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.Hints;

[Plugin(Name = "DayLight.Hints", Author = "Tiliboyy")]
public class PlayerHintsPlugin : DayLightCorePlugin<HintsConfig, HintsTranslation>
{
    
    public static bool DisableAllHints = false;

    public static bool DisableScpList = false;

    public static PlayerHintsPlugin Instance;
    protected override void Enabled()
    {                                            
        Instance = this;                                                                                       
        Player.EscapingPocketDimension += EventHandlers.EventHandlers.OnEscapingPocketDimension;                             
        Player.FailingEscapePocketDimension += EventHandlers.EventHandlers.OnFailingEscapePocketDimension;                   
        Player.EnteringPocketDimension += EventHandlers.EventHandlers.OnEnteringPocketDimension;                             
        Player.Died += EventHandlers.EventHandlers.OnDied;                                                                   
        Player.Verified += EventHandlers.EventHandlers.OnVerified;                                                           
    }
    public override void Disable()
    {
        Player.EscapingPocketDimension -= EventHandlers.EventHandlers.OnEscapingPocketDimension;
        Player.FailingEscapePocketDimension -= EventHandlers.EventHandlers.OnFailingEscapePocketDimension;
        Player.EnteringPocketDimension -= EventHandlers.EventHandlers.OnEnteringPocketDimension;
        Player.Died -= EventHandlers.EventHandlers.OnDied;
        Player.Verified -= EventHandlers.EventHandlers.OnVerified;
        Instance = null;
    }
}