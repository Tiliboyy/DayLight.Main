

using CommandSystem;
using DiscordSync.Plugin.Commands.RemoteAdmin.Subcommands;
using Exiled.Permissions.Extensions;

namespace DiscordSync.Plugin.Commands.RemoteAdmin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GameStoreParentCommand : ParentCommand
{
    public GameStoreParentCommand()
    {
        LoadGeneratedCommands();
    }

    public override string Command => "DiscordSync";

    public override string[] Aliases { get; } = { "ds" };

    public override string Description => "The DiscordSync parent command";

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new SyncRoles());
        RegisterCommand(new Toggle());

    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = AllCommands.Where(command => sender.CheckPermission($"DiscordSync.{command.Command}")).Aggregate("\nPlease enter a valid subcommand:",
            (current, command) => current + $"\n\n<color=#00fce3><b>- {command.Command} </b></color>\n<color=white>{command.Description}</color>");
        return false;
    }
}
