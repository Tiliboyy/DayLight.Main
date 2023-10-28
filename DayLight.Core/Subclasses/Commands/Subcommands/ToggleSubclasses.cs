using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace DayLight.Core.Subclasses.Commands.Subcommands;

internal class Toggle : ICommand
{
    public string Command { get; } = "toggle";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Enable/Disable subclasses";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("subclasses.toggle"))
        {
            response = $"You do not have the correct permission to use this command (subclasses.toggle)";

            return false;
        }

        if (!Manager.NoRandomRole)
        {
            Manager.NoRandomRole = true;
            response = $"Disabled Subclasses";
        }
        else
        {
            Manager.NoRandomRole = false;
            response = $"Enabled Subclasses";
        }

        return true;
    }
}