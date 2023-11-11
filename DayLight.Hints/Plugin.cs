using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.Hints.Configs;
using HarmonyLib;
using MEC;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.Hints;

[Plugin(Name = "DayLight.Hints", Author = "Tiliboyy")]
public class PlayerHintsPlugin : DayLightCoreModule<HintsConfig, HintsTranslation>
{
    
    public static bool DisableAllHints = false;

    public static bool DisableScpList = false;

    public static PlayerHintsPlugin Instance;
    public override void Enabled()
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