using CommandSystem;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;


namespace DayLight.Misc.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class ToggleRepsawn : CustomCommand
{
    public override string Command { get; } = "togglerespawns";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Enables/Disables team respawns";

    public override string Permission { get; } = "DU.togglerespawns";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {

        if (MiscPlugin.EnableTeamRespawns)
        {
            MiscPlugin.EnableTeamRespawns = false;
            commandResult.Response = "Respawns Disabled!";
            return;
        }
        MiscPlugin.EnableTeamRespawns = true;
        commandResult.Response = "Respawns Disabled!";

    }

}