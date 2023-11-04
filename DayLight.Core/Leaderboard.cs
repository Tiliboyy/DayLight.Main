using DayLight.Core.API.Database;
using DayLight.DiscordSync.Dependencys.Stats;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayLight.Core;

public class Leaderboard
{
    private static bool IsGenerated;
    private static string KillsLeaderboard;
    private static string DeathLeaderboard;
    private static string EscapeTimeLeaderboard;
    private static string UsedItemsLeaderboard;
    private static string ScpKillsLeaderboard;
    private static string RoundsLeaderboard;
    private static string PinkCandyLeaderboard;


    public static string GetLeaderboard(LeaderboardType leaderboardType)
    {
        if (!IsGenerated) return "Error";
        return leaderboardType switch
        {
            LeaderboardType.Kills => KillsLeaderboard,
            LeaderboardType.Deaths => DeathLeaderboard,
            LeaderboardType.EscapeTime => EscapeTimeLeaderboard,
            LeaderboardType.UsedItems => UsedItemsLeaderboard,
            LeaderboardType.PlayedRounds => RoundsLeaderboard,
            LeaderboardType.KilledSCPs => ScpKillsLeaderboard,
            LeaderboardType.PinkCandy => PinkCandyLeaderboard,

            _ => throw new ArgumentOutOfRangeException(nameof(leaderboardType), leaderboardType, null)
        };
    }

    public static LeaderboardType GetLeaderboardType(string input)
    {
        if(        Enum.TryParse<LeaderboardType>(input, out var loaderboardtype)
        )
        {
            return loaderboardtype;

        }
        else
        {
            return LeaderboardType.Kills;
        }
    }
    public static void UpdateLeaderboards()
    {
        try
        {
            IsGenerated = true;

            var statsPlayers = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players").FindAll().ToList();

            var sortedByKills = statsPlayers.OrderByDescending(p => p.Stats.Kills).Take(10);
            KillsLeaderboard = GenerateFormattedLeaderboard(sortedByKills, player => player.Kills, "Kills");

            var sortedByDeaths = statsPlayers.OrderByDescending(p => p.Stats.Deaths).Take(10);
            DeathLeaderboard = GenerateFormattedLeaderboard(sortedByDeaths, player => player.Deaths, "Tode");

            var sortedByEscapeTime = statsPlayers.OrderBy(p => p.Stats.FastestEscapeSeconds).Where(p => p.Stats.FastestEscapeSeconds != 0).Take(10);
            EscapeTimeLeaderboard = GenerateFormattedLeaderboard(sortedByEscapeTime, player => player.FastestEscapeSeconds, "Sekunden");

            var sortedByUsedItems = statsPlayers.OrderByDescending(p => p.Stats.UsedItems.Sum(item => item.Value)).Take(10);
            UsedItemsLeaderboard = GenerateFormattedLeaderboard(sortedByUsedItems, player => player.UsedItems.Sum(item => item.Value), "Items");

            var sortedByPlayedRounds = statsPlayers.OrderByDescending(p => p.Stats.PlayedRounds).Take(10);
            RoundsLeaderboard = GenerateFormattedLeaderboard(sortedByPlayedRounds, player => player.PlayedRounds, "Runden");

            var sortedByKilledSCPs = statsPlayers.OrderByDescending(p => p.Stats.KilledScps).Take(10);
            ScpKillsLeaderboard = GenerateFormattedLeaderboard(sortedByKilledSCPs, player => player.KilledScps, "Kills");

            var sortedByPinkCandyKills = statsPlayers.OrderByDescending(p => p.Stats.PinkCandyKills).Take(10);
            PinkCandyLeaderboard = GenerateFormattedLeaderboard(sortedByPinkCandyKills, player => player.PinkCandyKills, "Kills");
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw;
        }


    }

    private static string GenerateFormattedLeaderboard(IEnumerable<DatabasePlayer> sortedPlayers, Func<DiscordSync.Dependencys.Stats.Stats, double> valueSelector, string Suffix = "")
    {
        var enumerable = sortedPlayers as DatabasePlayer[] ?? sortedPlayers.ToArray();
        if (!enumerable.Any())
        {
            return "Empty";
        }
        var leaderboard = new StringBuilder();
        var rank = 1;
        var statsPlayers = sortedPlayers as DatabasePlayer[] ?? enumerable.ToArray();
        var maxNameLength = statsPlayers.Max(player => DayLightDatabase.GetNicknameFromSteam64ID(player.SteamID).Length);
        leaderboard.AppendLine("Rank".PadRight(5) + "Spieler".PadRight(maxNameLength + 5) + "Stat");
        leaderboard.AppendLine(new string('-', 40)); // Separator line

        foreach (var player in statsPlayers)
        {
            var nickname = DayLightDatabase.GetNicknameFromSteam64ID(player.SteamID);
            if (nickname == "none")
                nickname = player.SteamID;
            var rankstr = $"[{rank++.ToString()}]";
            var formattedLine = $"{rankstr,-5}{nickname.PadRight(maxNameLength + 5)}{Math.Round(valueSelector(player.Stats), 0)} {Suffix}" ;
            leaderboard.AppendLine(formattedLine);
        }

        return leaderboard.ToString();
    }
}