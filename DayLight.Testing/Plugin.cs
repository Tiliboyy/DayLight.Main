using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.Test;

[Plugin(Name = "DayLight.Testing", Author = "Tiliboyy")]
public class TestPlugin : DayLightCoreModule<TestConfig, TestTranslation>
{
    public static TestPlugin Instance;

    public override void Enabled()
    {
        Instance = this;
        Player.Verified += EventHandlers.Verified;
        Player.Died += EventHandlers.OnDeath;
    }
}