using DiscordSync.Plugin.Network;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace DiscordSync.Plugin;

public class DiscordSyncPlugin : Plugin<Config>
{
    public static DiscordSyncPlugin Instance;


    public static CancellationTokenSource NetworkCancellationTokenSource;
    public EventHandlers EventHandlers;
    public Network.Network Network;

    public override string Name { get; } = "DiscordSync";
    public override string Prefix { get; } = "DiscordSync";
    public override string Author { get; } = "Tiliboyy";


    public override void OnEnabled()
    {
        Instance = this;
        EventHandlers = new EventHandlers();
        Player.Verified += EventHandlers.OnVerified;
        Server.ReloadedRA += EventHandlers.OnReloadedRa;

        DiscordConnectionHandler.Start();

    }


    public override void OnDisabled()
    {
        Server.ReloadedRA -= EventHandlers.OnReloadedRa;
        Player.Verified -= EventHandlers.OnVerified;
        EventHandlers = null;
        DiscordConnectionHandler.Stop();
        Instance = null;
    }
}
