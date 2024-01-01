using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Neuron.Core.Meta;
using SCPUtils;
using System;
using System.Linq;

namespace DayLight.Stats.Commands;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
public class PlaytimeTop : CustomCommand
{

    public override string Command { get; } = "pttop";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Shows pttop";

    protected override void Respond(ArraySegment<string> arguments, Exiled.API.Features.Player player, ref CommandResult commandResult)
    {


        commandResult.Response = DayLightStatsPlugin.PlaytimeLeaderboard;

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
        DayLightStatsPlugin.PlaytimeLeaderboard = str;
    }
}
