#region

using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Dependencys.Stats;
using Exiled.API.Features;
using JetBrains.Annotations;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DayLight.Stat.Achievements;

public static class Achievements
{
    public static void Achive(this Player player, int id)
    {
        if (DiscordSyncStatsPlugin.DisableDiscordSyncStats) return;
        if (player.DoNotTrack) return;
        var achivement = Dependencys.Achievements.Achievements.AllAchivements.FirstOrDefault(x => x.Id == id);
        var dbplayer = player.GetAdvancedPlayer().DatabasePlayer;
        if (dbplayer != null && dbplayer.Stats.UnlockedAchievements.Contains(achivement.Id))
            return;
        player.SendHint(ScreenZone.Notifications, DiscordSyncStatsPlugin.Instance.Translation.SelfAchiveText.Replace("%achievement%", achivement.Name), DiscordSyncStatsPlugin.Instance.Config.BroadcastTime);
        foreach (var ply in Player.List.Where(x => x != player))
        {
            ply.SendHint(ScreenZone.Notifications,
                DiscordSyncStatsPlugin.Instance.Translation.AllAchiveText.Replace("%achievement%", achivement.Name)
                    .Replace("%player%", player.Nickname),
                DiscordSyncStatsPlugin.Instance.Config.BroadcastTime);
        }
        Timing.CallDelayed(0.5f, () => { player.GiveMoney(achivement.Reward); });
        player.GetAdvancedPlayer().DatabasePlayer.Stats.UnlockedAchievements.Add(achivement.Id);
    }
    [UsedImplicitly]
    public static List<Dependencys.Achievements.Achievements.Achivement> GetUnlockedAchivement(string steam64id)
    {
        try
        {
            var id = ulong.Parse(steam64id);
            var players = DayLightDatabase.Database.GetCollection<IDatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID == id);
            if (dbplayer == null)
                return new List<Dependencys.Achievements.Achievements.Achivement>();
            return dbplayer.Stats.UnlockedAchievements
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                .Select(achivement => Dependencys.Achievements.Achievements.AllAchivements.Where(x => x.Id == achivement))
                .Where(e => e.Count() != 0)
                .Select(e => e.First()).ToList();
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw;
        }
    }

}