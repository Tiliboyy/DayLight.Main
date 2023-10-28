using CommandSystem;
using System;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class Multiplier : ICommand
{
    public string Command { get; } = "multiplier";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Sets the Gamestore Money Multiplier";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = "Usage: gamestore multiplier <multiplier>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out var i))
        {
            response = "Usage: gamestore multiplier <multiplier>";
            return false;
        }
        GameStorePlugin.MoneyMuliplier = i;
        response = $"Der Multiplier wurde auf {i} gesetzt.";
        return true;
    }
}
