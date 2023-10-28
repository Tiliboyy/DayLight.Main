using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace DayLight.Core.Subclasses.Commands.Subcommands;

internal class ResetSubclassSpawns : ICommand
{
    public string Command { get; } = "reset";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Resets the amount of subclasses that have spawned";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {

        if (!sender.CheckPermission("subclasses.reset"))
        {
            response = $"You do not have the correct permission to use this command (subclasses.reset)";
            return true;
        }
        foreach (var subclass in Manager.Subclasses) subclass.SpawnedAmount = 0;

        response = "Done!";
        return true;
    }
}