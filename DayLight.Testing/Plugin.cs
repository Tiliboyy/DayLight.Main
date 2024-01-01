using DayLight.Core.API;
using Neuron.Core.Plugins;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.Test;

[Plugin(Name = "DayLight.Testing", Author = "Tiliboyy")]
public class TestPlugin : DayLightCorePlugin<TestConfig, TestTranslation>
{
    public static TestPlugin Instance;

    protected override void Enabled()
    {
        Instance = this;
        Player.Verified += EventHandlers.Verified;
        Player.Died += EventHandlers.OnDeath;
    }
}