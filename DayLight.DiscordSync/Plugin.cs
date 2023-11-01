using DiscordSync.Plugin.Network;
using Exiled.API.Features;
using JetBrains.Annotations;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace DiscordSync.Plugin;

[Plugin(Name = "DayLight.DiscordSync", Author = "Tiliboyy")]
public class DiscordSyncPlugin : ReloadablePlugin<DiscordSyncConfig, DiscordSyncTranslation>
{
    public static DiscordSyncPlugin Instance = null!;

    public static CancellationTokenSource NetworkCancellationTokenSource = null!;
    public Network.Network Network = null!;
    
    public override void EnablePlugin()
    {
        Instance = this;
        Player.Verified += EventHandlers.OnVerified;
        Server.ReloadedRA += EventHandlers.OnReloadedRa;

        DiscordConnectionHandler.Start();

    }


    public override void Disable()
    {
        Server.ReloadedRA -= EventHandlers.OnReloadedRa;
        Player.Verified -= EventHandlers.OnVerified;
        DiscordConnectionHandler.Stop();
        Instance = null!;
    }
}
