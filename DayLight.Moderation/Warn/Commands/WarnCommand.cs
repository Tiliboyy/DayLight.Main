#region

using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
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
    public override string Permission { get; } = "ws.addwarn";


    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult response)
    {

        if (arguments.Count < 3)
        {
            response.Response = "Usage: warn <Steam64ID@Steam> <Punkte> <Grund>";
            response.Success = false;
            return;
        }

        if (sender == null)
        {
                
            response.Response = "Something went wrong try again";
            response.Success = false;
            return;
        }

        var parsed = float.TryParse(arguments.At(1), out var number);
        if (!parsed)
        {
            response.Response = "Usage: warn <Steam64ID@Steam> <Punkte> <Grund>";
            response.Success = false;
            return;

        }

        if (arguments.At(0).Contains("@"))
        {
            response.Response = WarnDatabase.AddWarn(arguments.At(0), sender.Nickname, number,
                FormatArguments(arguments, 2), null);
            response.Success = true;
            return;
        }

 
        var playera = Player.Get(arguments.At(0));
        if (playera == null)
        {
            response.Response = "Spieler wurde nicht gefunden";
            response.Success = true;
            return;
        }
        playera?.Broadcast(ModerationSystemPlugin.Instance.Config.Broadcasttexttime, ModerationSystemPlugin.Instance.Config.Broadcasttext);
        response.Response = WarnDatabase.AddWarn(playera.UserId, sender.Nickname, number,
            FormatArguments(arguments, 2), null);
        response.Success = true;

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