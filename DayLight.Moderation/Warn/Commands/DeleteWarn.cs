#region

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.Permissions.Extensions;
using Neuron.Core.Meta;
using System;
using Player = Exiled.API.Features.Player;

#endregion

namespace DayLight.Moderation.Warn.Commands;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class delwarn : CustomCommand
{
    public override string Command { get; } = "removewarn";
    public override string[] Aliases { get; } = new string[] { "rwarn" };
    public override string Description { get; } = "Usage: removewarn <Steam64ID> <WARN ID>)";

    protected override string Permission { get; } = "ws.delete";
    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        if (arguments.Count != 2)
        {
            response = "Usage: removewarn <Steam64ID/ID> <WARN ID>";
            return true;
        }

        if (Player.TryGet(arguments.At(0), out var ply))
        {
            if (int.TryParse(arguments.At(1), out var id))
            {
                string e = WarnDatabase.RemoveWarn(ply.UserId, id);
                response = e;
                return true;
            }
            response = "ERROR: ID ist keine Zahl";
            return true;
        }

        if (int.TryParse(arguments.At(1), out var i))
        {
            string e = WarnDatabase.RemoveWarn(arguments.At(0), i);
            response = e;
            return true;
        }

        response = "ERROR: ID ist keine Zahl";
        return true;
    }
}