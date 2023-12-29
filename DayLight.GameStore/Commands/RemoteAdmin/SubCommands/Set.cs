using CommandSystem;
using DayLight.Core.API;
using DayLight.Core.API.CommandSystem;
using System;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class Set : CustomCommand
{
    public override string Command { get; } = "set";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Sets a players money";
    public override string Permission { get; } = "gs.set";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        if (arguments.Count != 2)
        {
            response.Response = "Usage: gamestore set <PlayerID> <Money>";
            response.Success = false;
            return;
        }

        if (player == null)
        {
            response.Response = "Player was not found";
            response.Success = false;
            return;
        }

        if (!float.TryParse(arguments.At(1), out var result))
        {
            response.Response = "Thats not a number!";
            response.Success = false;
            return;
        }

        var oldamount = player.GetMoney();
        if (player.SetMoney(result))
        {
            response.Response = $"Die {GameStorePlugin.Instance.Translation.CurrencyName} von {player.Nickname} wurde von {oldamount} auf {result} gesetzt";
            response.Success = true;
            return;

        }

        response.Response = $"Der Spieler wurde nicht gefunden oder er hat DNT aktiviert.";
    }
}
