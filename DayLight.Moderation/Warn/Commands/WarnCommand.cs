#region

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Neuron.Core.Meta;
using NorthwoodLib.Pools;
using System;

#endregion

namespace DayLight.Moderation.Warn.Commands;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class WarnCommand : CustomCommand
{
    public override string Command { get; } = "warn";
    public override string[] Aliases { get; } = new string[] { "WarnPlayer" };
    public override string Description { get; } = "Usage: warn <Steam64ID@steam/Spieler> <Punkte> <Grund>";
    protected override string Permission { get; } = "ws.addwarn";


    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        if (arguments.Count < 3)
        {
            response = "Usage: warn <Steam64ID@Steam> <Punkte> <Grund>";
            return false;
        }

        if (player == null)
        {
                
            response = "Something went wrong try again";
            return true;
        }

        var parsed = float.TryParse(arguments.At(1), out var number);
        if (!parsed)
        {
            response = "Usage: warn <Steam64ID@Steam> <Punkte> <Grund>";
            return false;

        }

        if (arguments.At(0).Contains("@"))
        {
            response = WarnDatabase.AddWarn(arguments.At(0), player.Nickname, number,
                FormatArguments(arguments, 2), null);
            return true;
        }

 
        var playera = Player.Get(arguments.At(0));
        if (playera == null)
        {
            response = "Spieler wurde nicht gefunden";
            return true;
        }
        playera?.Broadcast(ModerationSystemPlugin.Instance.Config.Broadcasttexttime, ModerationSystemPlugin.Instance.Config.Broadcasttext);

        response = WarnDatabase.AddWarn(playera.UserId, player.Nickname, number,
            FormatArguments(arguments, 2), null);
        return true;

    }


    public static string FormatArguments(ArraySegment<string> sentence, int index)
    {
        var sb = StringBuilderPool.Shared.Rent();
        foreach (string word in sentence.Segment(index))
        {
            sb.Append(word);
            sb.Append(" ");
        }

        string msg = sb.ToString();
        StringBuilderPool.Shared.Return(sb);
        return msg;
    }
}