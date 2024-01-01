using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using System;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class Multiplier : CustomCommand
{
    public override string Command { get; } = "multiplier";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Sets the Gamestore Money Multiplier";
    public override string Permission { get; } = "gs.multiplier";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response) 
    {
        if (arguments.Count != 1)
        {
            response.Response = "Usage: gamestore multiplier <multiplier>";
            response.Success = false;
            return;
        }

        if (!int.TryParse(arguments.At(0), out var i))
        {
            response.Response = "Usage: gamestore multiplier <multiplier>";
            response.Success = false;
            return;
        }
        GameStorePlugin.MoneyMuliplier = i;
        response.Response = $"Der Multiplier wurde auf {i} gesetzt.";
    }
}
