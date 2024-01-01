using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using System;

namespace DayLight.GameStore.Commands.RemoteAdmin.SubCommands;

internal class Toggle : CustomCommand
{
    public override string Command { get; } = "toggle";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Turn the GameStore on or off";
    public override string Permission { get; } = "gs.toggle";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        if (GameStorePlugin.EnableGamestore)
        {
            response.Response = "GameStore wurde deaktiviert";
            GameStorePlugin.EnableGamestore = false;
        }
        else
        {
            response.Response = "GameStore wurde aktiviert";
            GameStorePlugin.EnableGamestore = true;
        }
    }
}