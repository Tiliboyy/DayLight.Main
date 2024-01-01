#region

using DayLight.Core.API;
using DayLight.Stats.Achievements;
using DayLight.Stats.Commands;
using DayLight.Stats.Stats;
using Exiled.API.Features;
using MEC;
using Neuron.Core.Plugins;
using System.IO;

#endregion

namespace DayLight.Stats;

[Plugin(Name = "DayLight.Stats", Author = "Tiliboyy")]
public class DayLightStatsPlugin : DayLightCorePlugin<DayLightStatsConfig, DayLightStatsTranslation>
{
    public static DayLightStatsPlugin Instance;
    public static bool DisableSyncStats = false;
    public static string PlaytimeLeaderboard = "";
    protected override void Enabled()
    {
        Instance = this;
        TryCreateDirectory();
        Timing.CallDelayed(2f,
            () =>
            {
                PlaytimeTop.GetPTLeaderboard();
                Leaderboard.UpdateLeaderboards();

            });


        AchievementHandlers.RegisterEvents();
        StatsEventHandler.RegisterEvents();

    }
    public static void TryCreateDirectory()
    {
        if (!Directory.Exists(Path.Combine(Paths.Configs, "DiscordSync/")))
            Directory.CreateDirectory(Path.Combine(Paths.Configs, "DiscordSync/"));
    }

    public override void Disable()
    {
        AchievementHandlers.UnRegisterEvents();
        StatsEventHandler.UnRegisterEvents();
        Instance = null;
    }
}
