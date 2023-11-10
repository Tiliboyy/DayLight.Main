using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.GameStore.Configs;
using Exiled.Events.Handlers;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using System;
using System.IO;
using Paths = Exiled.API.Features.Paths;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.GameStore;

[Plugin(Name = "DayLight.GameStore", Author = "Tiliboyy", Version = "1.0.0")]
public class GameStorePlugin : DayLightCoreModule<GameStoreConfig, GameStoreTranslation>
{
    public static bool EnableGamestore = true;
    
    public static int MoneyMuliplier = 1;

    public static GameStorePlugin Instance;
    public override void Enabled()
    {
        try
        {
            Instance = this;
            Player.Dying += EventHandlers.EventHandlers.OnDying;
            Player.Escaping += EventHandlers.EventHandlers.OnEscaping;
            Player.Spawned += EventHandlers.EventHandlers.OnSpawned;
            Player.Verified += EventHandlers.EventHandlers.OnVerified;
            Player.UsedItem += EventHandlers.EventHandlers.OnUsedItem;
            Player.ThrownProjectile += EventHandlers.EventHandlers.OnThownItem;
            Player.EscapingPocketDimension += EventHandlers.EventHandlers.OnEscapingPocketDimension;
            Player.FailingEscapePocketDimension += EventHandlers.EventHandlers.OnFailingEscapePocketDimension;
            Player.EnteringPocketDimension += EventHandlers.EventHandlers.OnEnteringPocketDimension;
            Scp079.GainingLevel += EventHandlers.EventHandlers.OnGainingLevel;
        }
        catch (Exception e)
        {
            Logger.Error("Error: " + e);
        }
    }

    public override void Disable()
    {
        Player.Escaping -= EventHandlers.EventHandlers.OnEscaping;
        Player.Dying -= EventHandlers.EventHandlers.OnDying;
        Player.Spawned -= EventHandlers.EventHandlers.OnSpawned;
        Player.Verified -= EventHandlers.EventHandlers.OnVerified;
        Player.UsedItem -= EventHandlers.EventHandlers.OnUsedItem;
        Player.ThrownProjectile -= EventHandlers.EventHandlers.OnThownItem;
        Player.EscapingPocketDimension -= EventHandlers.EventHandlers.OnEscapingPocketDimension;
        Player.FailingEscapePocketDimension -= EventHandlers.EventHandlers.OnFailingEscapePocketDimension;
        Player.EnteringPocketDimension -= EventHandlers.EventHandlers.OnEnteringPocketDimension;
        Scp079.GainingLevel -= EventHandlers.EventHandlers.OnGainingLevel;
        Instance = null;
    }
}