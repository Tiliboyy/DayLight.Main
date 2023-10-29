using CommandSystem;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace DayLight.Core.Subclasses.Commands.Subcommands;

internal class ResetSubclassSpawns : CustomCommand
{
    public override string Command { get; } = "reset";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Resets the amount of subclasses that have spawned";

    protected override string Permission { get; } = "subclasses.reset";
    protected override bool Respond(ArraySegment<string> arguments, Player sender, out string response)
    {
        
        foreach (var subclass in Manager.Subclasses) subclass.SpawnedAmount = 0;

        response = "Done!";
        return true;
    }
}