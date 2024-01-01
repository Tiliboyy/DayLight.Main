using CommandSystem;
using DayLight.Core.API;
using DayLight.Core.API.CommandSystem;
using System;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;
[CommandHandler(typeof(GameStoreParentCommand))]
internal class Add : CustomCommand
{
    public override string Command { get; } = "add";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Gives a player money";

    public override string Permission { get; } = "gs.add";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {


        if (arguments.Count != 2)
        {
            commandResult.Response = "Usage: gamestore Add <<PlayerID> or <all / *>> <amount>";
            commandResult.Success = false;
            return;
        }

        switch (arguments.At(0))
        {
            case "*":
            case "all":
                if (!int.TryParse(arguments.At(1), out var amount) && amount <= 0)
                {
                    commandResult.Response = "Thats not a number!";
                    commandResult.Success = false;
                    return;
                }

                foreach (var ply in Player.List)
                    ply.GiveMoney(amount);

                commandResult.Response = $"Everyone was given {amount} {GameStorePlugin.Instance.Translation.CurrencyName}";
                commandResult.Success = true;
                return;
            default:
                var pl = Player.Get(arguments.At(0));
                if (pl == null)
                {
                    commandResult.Response = $"Player not found: {arguments.At(0)}";
                    commandResult.Success = false;
                    return;
                }

                if (!int.TryParse(arguments.At(1), out var amountsingle) && amountsingle <= 0)
                {
                    commandResult.Response = $"Money argument invalid: {arguments.At(1)}";
                    commandResult.Success = false;
                    return;
                }
                pl.GiveMoney(amountsingle);

                commandResult.Response =
                    $"Player {pl.Nickname} has been given {amountsingle} {GameStorePlugin.Instance.Translation.CurrencyName}";
                commandResult.Success = true;
                break;
        }
    }
}