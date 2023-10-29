using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace DiscordSync.Plugin.Commands.RemoteAdmin.Subcommands;

internal class SyncRoles : ICommand
{
    public string Command { get; } = "syncroles";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Syncs you roles used for debug";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
        if (!player.CheckPermission("DiscordSync.Debug"))
        {
            response = "You do not have the required permission for this command";
            return true;
        }

        _ = EventHandlers.AssignRole(player);
        response = "Synced!";
        return true;
    }
}
