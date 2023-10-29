#region

using DayLight.Stat.Achievements;
using DayLight.Stat.Commands;
using DayLight.Stat.Stats;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using System;
using System.IO;

#endregion

namespace DayLight.Stat;

public class DiscordSyncStatsPlugin : Plugin<Config, Translation>
{
    public static DiscordSyncStatsPlugin Instance;
    public static bool DisableDiscordSyncStats = false;
    public static string PlaytimeLeaderboard = "";
    public AchievementHandlers AchievementHandlers;
    public Harmony Harmony;
    public StatsEventHandler StatsEventHandler;
    public override string Author => "Tiliboyy";
    public override string Name => "DiscordSync.Stats";
    public override string Prefix => "DiscordSync.Stats";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredExiledVersion => new(6, 0, 0, 0);
    public override void OnEnabled()
    {
        //Harmony = new Harmony("DiscordSync.Stats.Tiliboyy.Patches");
        //Harmony.PatchAll();
        Instance = this;
        StatsEventHandler = new StatsEventHandler();
        AchievementHandlers = new AchievementHandlers();
        TryCreateDirectory();
        StatsEventHandler.RegisterEvents();
        Timing.CallDelayed(2f,
            () =>
            {
                PlaytimeTop.GetPTLeaderboard();
                Leaderboard.UpdateLeaderboards();

            });

        //Achivement Events
        AchievementHandlers.RegisterEvents();
    }

    public static void TryCreateDirectory()
    {
        if (!Directory.Exists(Path.Combine(Paths.Configs, "DiscordSync/")))
            Directory.CreateDirectory(Path.Combine(Paths.Configs, "DiscordSync/"));
    }

    public override void OnDisabled()
    {
        AchievementHandlers.UnRegisterEvents();
        StatsEventHandler.UnRegisterEvents();

        Harmony.UnpatchAll();
        Instance = null;
        StatsEventHandler = null;
    }
}
