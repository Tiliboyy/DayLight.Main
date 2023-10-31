using CommandSystem;
using SCPUtils;
using System;
using System.Linq;

namespace DayLight.Stat.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class PlaytimeTop : ICommand
{

    public string Command { get; } = "pttop";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Shows pttop";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {


        response = DiscordSyncStatsPlugin.PlaytimeLeaderboard;
        return true;

    }
    public static void GetPTLeaderboard(int amount = 10)
    {
        var players = Database.LiteDatabase.GetCollection<Player>();
        var e = players.FindAll().OrderByDescending(x => new TimeSpan(0, 0, x.PlayTimeRecords.Sum(e => e.Value))).Take(amount).ToList();
        var i = 1;
        var str = "\n";
        foreach (var player in e)
        {
            if (player.Name == null)
            {
                str += $"\n[{i}] {player.Id}: {new TimeSpan(0, 0, player.PlayTimeRecords.Sum(pt => pt.Value)).ToString()}";
            }
            else
            {
                str += $"\n[{i}] {player.Name}: {new TimeSpan(0, 0, player.PlayTimeRecords.Sum(pt => pt.Value)).ToString()}";
            }

            i++;
        }
        DiscordSyncStatsPlugin.PlaytimeLeaderboard = str;
    }
}
