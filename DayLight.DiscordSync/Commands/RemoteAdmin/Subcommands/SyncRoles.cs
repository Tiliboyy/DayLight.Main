using CommandSystem;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace DiscordSync.Plugin.Commands.RemoteAdmin.Subcommands;

internal class SyncRoles : CustomCommand
{
    public override string Command { get; } = "syncroles";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Syncs you roles used for debug";

    public override string Permission { get; } = "DiscordSync.Debug";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {

        _ = EventHandlers.AssignRole(player);
        response.Response = "Synced!";
        response.Success = true;
        
    }
    
}
