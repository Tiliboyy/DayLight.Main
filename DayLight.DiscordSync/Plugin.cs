using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DiscordSync.Plugin.Network;
using JetBrains.Annotations;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace DiscordSync.Plugin;

[Plugin(Name = "DayLight.DiscordSync", Author = "Tiliboyy")]
public class DiscordSyncPlugin : DayLightCorePlugin<DiscordSyncConfig, DiscordSyncTranslation>
{
    public static DiscordSyncPlugin Instance;

    public static CancellationTokenSource NetworkCancellationTokenSource = null!;
    public Network.Network Network = null!;
    public static bool DisableDiscordSyncStats { get; set; } = false;

    protected override void Enabled()
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
