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

    protected override string Permission { get; } = "DU.togglerespawns";

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        if (MiscPlugin.EnableTeamRespawns)
        {
            MiscPlugin.EnableTeamRespawns = false;
            response = "Respawns Disabled!";
            return true;
        }
        MiscPlugin.EnableTeamRespawns = true;
        response = "Respawns Enabled!";
        return true;

    }

}