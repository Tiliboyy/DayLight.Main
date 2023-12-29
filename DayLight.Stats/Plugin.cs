#region

using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.Stat.Achievements;
using DayLight.Stat.Commands;
using DayLight.Stat.Stats;
using Exiled.API.Features;
using MEC;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using System.IO;

#endregion

namespace DayLight.Stat;

[Plugin(Name = "DiscordSync.Stats", Author = "Tiliboyy")]
public class DiscordSyncStatsPlugin : DayLightCorePlugin<Config, StatsTranslation>
{
    public static DiscordSyncStatsPlugin Instance;
    public static bool DisableDiscordSyncStats = false;
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
