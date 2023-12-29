using CommandSystem;
using DayLight.Core.API.CommandSystem;
using DayLight.Core.API.Features;
using System;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class ResetLimits : CustomCommand
{
    public override string Command { get; } = "resetlimits";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Resets a players buy Limits";

    public override string Permission { get; } = "gs.resetlimits";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        if (arguments.Count != 1)
        {
            response.Response = "Usage: gamestore resetlimits <PlayerID>";
            response.Success = false;
            return;
        }

        if (player == null)
        {
            response.Response = "Player was not found";
            response.Success = true;
            return;

        }
        
        AdvancedPlayer.Get(player)?.ResetGameStoreLimits();
        response.Response = $"Limits reset!";
        response.Success = true;

    }
}
