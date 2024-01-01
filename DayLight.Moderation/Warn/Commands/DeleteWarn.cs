#region

using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
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

    public override string Permission { get; } = "ws.delete";
    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult response)
    {

        if (arguments.Count != 2)
        {
            response.Response = "Usage: removewarn <Steam64ID/ID> <WARN ID>";
            response.Success = true;
            return;
        }

        if (Player.TryGet(arguments.At(0), out var ply))
        {
            if (int.TryParse(arguments.At(1), out var id))
            {
                string e = WarnDatabase.RemoveWarn(ply.UserId, id);
                response.Response = e;
                response.Success = true;
                return;
            }
            response.Response = "ERROR: ID ist keine Zahl";
            response.Success = true;
            return;
        }

        if (int.TryParse(arguments.At(1), out var i))
        {
            string e = WarnDatabase.RemoveWarn(arguments.At(0), i);
            response.Response = e;
            response.Success = true;
            return;
        }

        response.Response = "ERROR: ID ist keine Zahl";
        response.Success = true;
    }
}