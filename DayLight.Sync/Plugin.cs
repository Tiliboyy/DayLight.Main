using DayLight.Core.API;
using DayLight.Sync.Network;
using Neuron.Core.Plugins;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace DayLight.Sync;

[Plugin(Name = "DayLight.Sync", Author = "Tiliboyy")]
public class DayLightSyncPlugin : DayLightCorePlugin<DayLightSyncConfig, DayLightSyncTranslation>
{
    public static DayLightSyncPlugin Instance = null!;

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
