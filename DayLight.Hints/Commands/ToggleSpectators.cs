using CommandSystem;
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

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {
        if (NoSpectateList.Contains(player))
        {
            response = "Die Spectator Liste wurde aktiviert";
            NoSpectateList.Remove(player);
        }
        else
        {
            response = "Die Spectator Liste wurde deaktiviert";
            NoSpectateList.Add(player);
        }
        return true;
    }
}