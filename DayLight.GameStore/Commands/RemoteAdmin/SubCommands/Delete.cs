using DayLight.Core.API.CommandSystem;
using DayLight.Core.API.Database;
using System;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class Delete : CustomCommand
{
    public override string Command { get; } = "delete";

    public override string[] Aliases { get; } = new []{"del"};

    public override string Description { get; } = "Deletes a Players database entry";

    public override string Permission { get; } = "gs.delete";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        if (arguments.Count >= 1)
        {
            response.Response = "Usage: gamestore delete <player>";
            response.Success = false;
            return;
        }

        if (player == null)
        {
            response.Response = "Player was not found";
            response.Success = false;
            return;

        }

        if (arguments.Count == 2)
        {
            if (arguments.At(2) != "CONFIRM")
            {
                response.Response = "WARNIMG: You are about to delete the Database entry of " + player.Nickname +
                                    ",if you are sure you want to do that type gamestore delete <player> CONFIRM";
                response.Success = true;
                return;
            }
            DayLightDatabase.RemovePlayer(player);
            response.Response = $"{player.Nickname}'s database entry was removed!";
            response.Success = true;
            return;

        }
        response.Response = "WARNIMG: You are about to delete the Database entry of " + player.Nickname +
                   ",if you are sure you want to do that type gamestore delete <player> CONFIRM";
        response.Success = false;

    }
}
