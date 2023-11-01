using Exiled.API.Features;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Player = Exiled.Events.Handlers.Player;

namespace DayLight.Test
{
    [Plugin(Name = "DayLight.Testing", Author = "Tiliboyy")]
    public class TestPlugin : ReloadablePlugin<TestConfig, TestTranslation>
    {
        public static TestPlugin Instance;

        public override void EnablePlugin()
        {
            Instance = this;
            Player.Verified += EventHandlers.Verified;
            Player.Died += EventHandlers.OnDeath;
            base.EnablePlugin();
        }
    }
}
