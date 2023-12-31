using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Core.API.Database;
using Exiled.API.Features;
using Neuron.Core.Meta;
using System;

namespace DayLight.GameStore.Commands.ClientConsole;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
internal class Baltop : CustomCommand
{
    public override string Command { get; } = "Baltop";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Shows Baltop";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {
        commandResult.Response = DayLightDatabase.Stats.GetPlayerLeaderboard(player);
    }
}