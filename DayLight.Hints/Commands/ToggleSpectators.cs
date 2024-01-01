using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace DayLight.Hints.Commands;

[Command(new [] { Platform.ClientConsole })]
internal class ToggleSpectators : CustomCommand
{
    public static List<Player> NoSpectateList = new();
    public override string Command { get; } = "togglespectators";

    public override string[] Aliases { get; } = { "ts" };

    public override string Description { get; } = "Toggles the Spectator List";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {
        if (NoSpectateList.Contains(player))
        {
            commandResult.Response = "Die Spectator Liste wurde aktiviert";
            NoSpectateList.Remove(player);
        }
        else
        {
            commandResult.Response = "Die Spectator Liste wurde deaktiviert";
            NoSpectateList.Add(player);
        }
    }
}