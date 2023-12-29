#region

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Neuron.Core.Meta;
using System;

#endregion

namespace DayLight.Moderation.Warn.Commands;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class Getwarns : CustomCommand
{
    public override string Command { get; } = "getwarns";
    public override string[] Aliases { get; } = new string[] { "gwarns", "showwarns" };
    public override string Description { get; } = "Usage: getwarns <ID/SteamID> <Optional: true/false>)";

    public override string Permission { get; } = "ws.getwarns";


    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult response)
    {


        var onlynew = true;
        switch (arguments.Count)
        {
            case 0:
                response.Response = "Usage: getwarns <ID/SteamID>";
                response.Success = false;
                return;
            case >= 2 when bool.TryParse(arguments.At(1), out var onlynewb):
            {
                onlynew = !onlynewb;
                break;
            }
            case >= 2:
                onlynew = true;
                break;
        }

        string e;
        if (arguments.At(0).Contains("@"))
        {
            e = WarnDatabase.GetWarns(arguments.At(0), onlynew, out bool b);
            if (b)
            {
                e = e.Insert(0, "\nVerwarnungen");
            }
            response.Response = e;
            response.Success = true;
            return;
        }

        if (int.TryParse(arguments.At(0), out var id))
        {
            var ply = Player.Get(id);
            if (ply == null)
            {
                response.Response = "Spieler wurde nicht gefunden";
                response.Success = false;
                return;
            }

            e = WarnDatabase.GetWarns(ply.UserId, onlynew, out bool b);
            if (b)
            {
                e = e.Insert(0, "\nVerwarnungen von " + ply.Nickname);
            }
            response.Response = e;
            response.Success = true;
            return;
        }


        response.Response = "Spieler wurde nicht gefunden";
        response.Success = false;
    }
}