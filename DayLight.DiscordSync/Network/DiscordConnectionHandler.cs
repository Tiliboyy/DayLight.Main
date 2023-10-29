using DiscordSync.Plugin.Commands.ClientConsole;
using Exiled.API.Features;
using MEC;

namespace DiscordSync.Plugin.Network;

public class DiscordConnectionHandler
{

    internal static CoroutineHandle LinkTimer;
    internal static CoroutineHandle AliveChecks;
    internal static void Start()
    {
        try
        {


            DiscordSyncPlugin.Instance.Network = new Network(DiscordSyncPlugin.Instance.Config.IpAdress, DiscordSyncPlugin.Instance.Config.ServerPort, TimeSpan.FromSeconds(5));

            DiscordSyncPlugin.NetworkCancellationTokenSource = new CancellationTokenSource();
            _ = DiscordSyncPlugin.Instance.Network.Start(new CancellationTokenSource());
            DiscordSyncPlugin.Instance.Network.ReceivedFull += NetworkEventHandler.DataReceived;
        }
        catch (Exception e)
        {
            Log.Error("Failed to connect to Server: " + e);
            return;
        }
        if (!Directory.Exists(Path.Combine(Paths.Configs, "DiscordSync/")))
            Directory.CreateDirectory(Path.Combine(Paths.Configs, "DiscordSync/"));
        LinkTimer = Timing.RunCoroutine(LinkCommand.Timer(), "Checks");
        AliveChecks = Timing.RunCoroutine(AliveCheck(), "AliveCheck");
        Log.Info("Started Client!");

        Link.LinkDatabase.ReadYaml();

    }

    internal static void Stop()
    {
        try
        {
            DiscordSyncPlugin.Instance.Network.ReceivedFull -= NetworkEventHandler.DataReceived;
            DiscordSyncPlugin.Instance.Network.Dispose();
            DiscordSyncPlugin.NetworkCancellationTokenSource = null;

            DiscordSyncPlugin.Instance.Network = null;

        }
        catch (Exception e)
        {
            Log.Error("Failed to connect to Server: " + e);
            return;
        }
        if (!Directory.Exists(Path.Combine(Paths.Configs, "DiscordSync/")))
            Directory.CreateDirectory(Path.Combine(Paths.Configs, "DiscordSync/"));
        Timing.KillCoroutines(LinkTimer);
        Timing.KillCoroutines(AliveChecks);
        Link.LinkDatabase.ReadYaml();

    }

    public static IEnumerator<float> AliveCheck()
    {
        var checks = 0;
        for (;;)
        {
            checks++;
            if (!DiscordSyncPlugin.Instance.Network.IsConnected)
            {
                if (checks > 4)
                {
                    Log.Warn("No server connected!");
                }
            }
            yield return Timing.WaitForSeconds(2.5f);

        }
    }
}
