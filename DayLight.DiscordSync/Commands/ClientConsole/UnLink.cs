using CommandSystem;
using Exiled.API.Features;

namespace DiscordSync.Plugin.Commands.ClientConsole;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
internal class UnLink : ICommand
{
    public string Command { get; } = "unlink";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Unlinkt deinen Discord Account";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        ulong.TryParse(Player.Get(sender).RawUserId, out var result);
        response = Link.LinkDatabase.Unlink(result) ? "Unlinked!" : "Du bist nicht verlinkt!";
        return true;
    }
}
