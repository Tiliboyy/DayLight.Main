#region

using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Dependency.Models;
using Exiled.API.Features;
using JetBrains.Annotations;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DayLight.Stats.Achievements;

public static class Achievements
{
    public static void Achive(this Player player, int id)
    {
        if (DayLightStatsPlugin.DisableSyncStats) return;
        if (player.DoNotTrack) return;
        var achivement = Dependency.Models.Achievements.AllAchivements.FirstOrDefault(x => x.Id == id);
        var dbplayer = player.GetAdvancedPlayer().DatabasePlayer;
        if (dbplayer != null && dbplayer.Stats.UnlockedAchievements.Contains(achivement.Id))
            return;
        player.SendHint(ScreenZone.Notifications, DayLightStatsPlugin.Instance.Translation.SelfAchiveText.Replace("%achievement%", achivement.Name), DayLightStatsPlugin.Instance.Config.BroadcastTime);
        foreach (var ply in Player.List.Where(x => x != player))
        {
            ply.SendHint(ScreenZone.Notifications,
                DayLightStatsPlugin.Instance.Translation.AllAchiveText.Replace("%achievement%", achivement.Name)
                    .Replace("%player%", player.Nickname),
                DayLightStatsPlugin.Instance.Config.BroadcastTime);
        }
        Timing.CallDelayed(0.5f, () => { player.GiveMoney(achivement.Reward); });
        player.GetAdvancedPlayer().DatabasePlayer.Stats.UnlockedAchievements.Add(achivement.Id);
    }
    [UsedImplicitly]
    public static List<Dependency.Models.Achievements.Achivement> GetUnlockedAchivement(string steam64id)
    {
        try
        {
            var id = ulong.Parse(steam64id);
            var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID == id);
            if (dbplayer == null)
                return new List<Dependency.Models.Achievements.Achivement>();
            return dbplayer.Stats.UnlockedAchievements
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                .Select(achivement => Dependency.Models.Achievements.AllAchivements.Where(x => x.Id == achivement))
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