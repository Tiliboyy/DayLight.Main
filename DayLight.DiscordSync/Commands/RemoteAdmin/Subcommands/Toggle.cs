using CommandSystem;
using Exiled.Permissions.Extensions;

namespace DiscordSync.Plugin.Commands.RemoteAdmin.Subcommands;

internal class Toggle : ICommand
{
    public string Command { get; } = "toggle";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Turn Stats on or off";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission($"DiscordSync.{Command}"))
        {
            response = "You do not have permission to use this command";
            return false;
        }
        /*
        if (!DiscordSyncStatsPlugin.DisableDiscordSyncStats)
        {
            response = "Stats wurde deaktiviert";
            DiscordSyncStatsPlugin.DisableDiscordSyncStats = true;
        }
        else
        {
            response = "Stats wurde aktiviert";
            DiscordSyncStatsPlugin.DisableDiscordSyncStats = false;
        }
*/
        response = "Stats wurde aktiviert";
        return true;
    }
}
