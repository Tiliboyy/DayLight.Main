using Exiled.API.Features;
using MEC;

namespace DayLight.Sync.Network;

public class DiscordConnectionHandler
{

    internal static CoroutineHandle LinkTimer;
    internal static CoroutineHandle AliveChecks;
    internal static void Start()
    {
        try
        {


            DayLightSyncPlugin.Instance.Network = new Network(DayLightSyncPlugin.Instance.Config.IpAdress, DayLightSyncPlugin.Instance.Config.ServerPort, TimeSpan.FromSeconds(5));

            DayLightSyncPlugin.NetworkCancellationTokenSource = new CancellationTokenSource();
            _ = DayLightSyncPlugin.Instance.Network.Start(new CancellationTokenSource());
            DayLightSyncPlugin.Instance.Network.ReceivedFull += NetworkEventHandler.DataReceived;
        }
        catch (Exception e)
        {
            Log.Error("Failed to connect to Server: " + e);
            return;
        }
        LinkTimer = Timing.RunCoroutine(Commands.ClientConsole.Link.Timer(), "Checks");
        AliveChecks = Timing.RunCoroutine(AliveCheck(), "AliveCheck");
        Log.Info("Started Client!");
        
    }

    internal static void Stop()
    {
        try
        {
            DayLightSyncPlugin.Instance.Network.ReceivedFull -= NetworkEventHandler.DataReceived;
            DayLightSyncPlugin.Instance.Network.Dispose();
            DayLightSyncPlugin.NetworkCancellationTokenSource = null;

            DayLightSyncPlugin.Instance.Network = null;

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
    }

    public static IEnumerator<float> AliveCheck()
    {
        var checks = 0;
        for (;;)
        {
            checks++;
            if (!DayLightSyncPlugin.Instance.Network.IsConnected)
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
